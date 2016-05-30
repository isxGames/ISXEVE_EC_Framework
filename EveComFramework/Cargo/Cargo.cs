using System;
using System.Collections.Generic;
using System.Linq;
using EveCom;
using EveComFramework.Core;

namespace EveComFramework.Cargo
{
    /// <summary>
    /// This class handles cargo operation, including navigation
    /// </summary>
    public class Cargo : State
    {
        private class CargoAction
        {
            internal Func<object[], bool> Action { get; set; }
            internal Bookmark Bookmark { get; set; }
            internal Func<Item, bool> QueryString { get; set; }
            internal int Quantity { get; set; }
            internal Func<InventoryContainer> Source { get; set; }
            internal string Container { get; set; }
            internal Func<InventoryContainer> Target { get; set; }
            internal bool Compress { get; set; }

            internal CargoAction(Func<object[], bool> Action, Bookmark Bookmark, Func<InventoryContainer> Source, string Container, Func<Item, bool> QueryString, int Quantity, Func<InventoryContainer> Target, bool Compress=false)
            {
                this.Action = Action;
                this.Bookmark = Bookmark;
                this.Source = Source;
                this.Container = Container;
                this.QueryString = QueryString;
                this.Quantity = Quantity;
                this.Target = Target;
                this.Compress = Compress;
            }

            public CargoAction Clone()
            {
                return new CargoAction(Action, Bookmark, Source, Container, QueryString, Quantity, Target, Compress);
            }
        }

        #region Variables

        LinkedList<CargoAction> CargoQueue = new LinkedList<CargoAction>();
        CargoAction CurrentCargoAction;
        CargoAction BuildCargoAction;
        Move.Move Move = EveComFramework.Move.Move.Instance;
        /// <summary>
        /// Log for Cargo module
        /// </summary>
        public Logger Log = new Logger("Cargo");

        #endregion

        #region Instantiation

        static Cargo _Instance;
        /// <summary>
        /// Singletoner
        /// </summary>
        public static Cargo Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new Cargo();
                }
                return _Instance;
            }
        }

        private Cargo()
        {

        }

        #endregion

        #region Actions

        /// <summary>
        /// Specify the location to perform the cargo operation
        /// </summary>
        /// <param name="Bookmark">Bookmark object for the location to perform the cargo operation</param>
        /// <param name="Source">InventoryContainer to use for the operation (load source for Load, unload destination for Unload)  Default: Station Item Hangar</param>
        /// <param name="ContainerName">Name of the entity to use for the operation (for entities with inventory containers in space)</param>
        /// <returns></returns>
        public Cargo At(Bookmark Bookmark, Func<InventoryContainer> Source = null, string ContainerName = "")
        {
            BuildCargoAction = new CargoAction(null, Bookmark, Source ?? (() => Station.ItemHangar), ContainerName, null, 0, null);
            return this;
        }

        /// <summary>
        /// Add a Load operation
        /// </summary>
        /// <param name="QueryString">Linq parameters for specifying the items to load.</param>
        /// <param name="Quantity">Quantity of the item to load (Must specify a single item type using QueryString)</param>
        /// <param name="Target">Where to load the item(s) - Default: Cargo Hold</param>
        /// <returns></returns>
        public Cargo Load(Func<Item, bool> QueryString = null, int Quantity = 0, Func<InventoryContainer> Target = null)
        {
            BuildCargoAction.Action = Load;
            BuildCargoAction.QueryString = QueryString ?? (item => true);
            BuildCargoAction.Quantity = Quantity;
            BuildCargoAction.Target = Target ?? (() => MyShip.CargoBay);
            CargoQueue.AddFirst(BuildCargoAction.Clone());
            if (Idle) QueueState(Process);
            return this;
        }

        /// <summary>
        /// Add an Unload operation
        /// </summary>
        /// <param name="QueryString">Linq parameters for specifying the items to unload.</param>
        /// <param name="Quantity">Quantity of the item to unload (Must specify a single item type using QueryString)</param>
        /// <param name="Target">Where to unload the item(s) from - Default: Cargo Hold</param>
        /// <param name="Compress">Compress all compressible items after unloading. Only applicable if target is a compression array - Default: false</param>
        /// <returns></returns>
        public Cargo Unload(Func<Item, bool> QueryString = null, int Quantity = 0, Func<InventoryContainer> Target = null, bool Compress=false)
        {
            BuildCargoAction.Action = Unload;
            BuildCargoAction.QueryString = QueryString ?? (item => true);
            BuildCargoAction.Quantity = Quantity;
            BuildCargoAction.Target = Target ?? (() => MyShip.CargoBay);
            BuildCargoAction.Compress = Compress;
            CargoQueue.AddFirst(BuildCargoAction.Clone());
            if (Idle) QueueState(Process);
            return this;
        }

        /// <summary>
        /// Don't do anything - used in conjunction with Cargo.At to queue up a move to a location without performing a cargo operation
        /// </summary>
        /// <returns></returns>
        public Cargo NoOp()
        {
            BuildCargoAction.Action = NoOp;
            BuildCargoAction.QueryString = null;
            BuildCargoAction.Quantity = 0;
            BuildCargoAction.Target = null;
            CargoQueue.AddFirst(BuildCargoAction.Clone());
            if (Idle) QueueState(Process);
            return this;
        }

        #endregion

        #region States

        bool Process(object[] Params)
        {
            if (CargoQueue.Any())
            {
                CurrentCargoAction = CargoQueue.Last();
                CargoQueue.RemoveLast();
            }
            else
            {
                CurrentCargoAction = null;
                return true;
            }

            Move.Bookmark(CurrentCargoAction.Bookmark);

            QueueState(Traveling);
            QueueState(WarpFleetMember);
            QueueState(Traveling);
            QueueState(CurrentCargoAction.Action);
            if (CurrentCargoAction.Compress)
            {
                QueueState(Stack);
                QueueState(Compress);
            }
            QueueState(Stack);
            QueueState(Process);

            return true;
        }

        bool Traveling(object[] Params)
        {
            if (!Move.Idle)
            {
                return false;
            }
            return true;
        }

        bool WarpFleetMember(object[] Params)
        {
            return true;
        }

        bool Compress(object[] Params)
        {
            if (!Entity.All.Any(a => a.GroupID == Group.CompressionArray && a.SurfaceDistance < 3000) || Entity.All.Any(a => a.GroupID == Group.CompressionArray && a.SurfaceDistance >= 3000)) return true;

            foreach (Item item in CurrentCargoAction.Source().Items.Where(a => a.Compressible && (a.GroupID == Group.Ice || (a.CategoryID == Category.Asteroid && a.Quantity > 100))))
            {
                item.Compress();
                return false;
            }
            return true;
        }

        bool Stack(object[] Params)
        {
            try
            {
                if (CurrentCargoAction.Action == Load)
                {
                    CurrentCargoAction.Target().StackAll();
                }
                if (CurrentCargoAction.Action == Unload)
                {
                    CurrentCargoAction.Source().StackAll();
                }

            }
            catch { }
            return true;
        }

        bool Load(object[] Params)
        {
            if (!CurrentCargoAction.Target().IsPrimed)
            {
                CurrentCargoAction.Target().Prime();
                return false;
            }
            if (!CurrentCargoAction.Source().IsPrimed)
            {
                CurrentCargoAction.Source().Prime();
                return false;
            }
            Log.Log("|oLoading");
            try
            {
                if (CurrentCargoAction.Quantity != 0)
                {
                    int DesiredQuantity = CurrentCargoAction.Quantity;
                    InventoryContainer Target = CurrentCargoAction.Target();
                    foreach (Item item in CurrentCargoAction.Source().Items.Where(CurrentCargoAction.QueryString))
                    {
                        if (item.Quantity < DesiredQuantity)
                        {
                            DesiredQuantity -= item.Quantity;
                            Target.Add(item);
                        }
                        else
                        {
                            Target.Add(item, DesiredQuantity);
                            return true;
                        }
                    }
                }
                else
                {
                    double AvailableSpace = CurrentCargoAction.Target().MaxCapacity - CurrentCargoAction.Target().UsedCapacity;
                    foreach (Item item in CurrentCargoAction.Source().Items.Where(CurrentCargoAction.QueryString))
                    {
                        if (item.Quantity * item.Volume <= AvailableSpace)
                        {
                            CurrentCargoAction.Target().Add(item);
                            AvailableSpace = AvailableSpace - item.Quantity * item.Volume;
                        }
                        else
                        {
                            int nextMove = (int)Math.Floor(AvailableSpace / item.Volume);
                            CurrentCargoAction.Target().Add(item, nextMove);
                            AvailableSpace = AvailableSpace - nextMove * item.Volume;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Log(ex.Message);
            }
            return true;
        }

        bool Unload(object[] Params)
        {
            if (!CurrentCargoAction.Target().IsPrimed)
            {
                Log.Log("Calling Prime() on Target", LogType.DEBUG);
                CurrentCargoAction.Target().Prime();
                return false;
            }
            if (!CurrentCargoAction.Source().IsPrimed)
            {
                Log.Log("Calling Prime() on Source", LogType.DEBUG);
                CurrentCargoAction.Source().Prime();
                return false;
            }
            Log.Log("|oUnloading");
            try
            {
                if (CurrentCargoAction.Quantity != 0)
                {
                    int DesiredQuantity = CurrentCargoAction.Quantity;
                    InventoryContainer Target = CurrentCargoAction.Source();
                    foreach (Item item in CurrentCargoAction.Target().Items.Where(CurrentCargoAction.QueryString))
                    {
                        if (item.Quantity < DesiredQuantity)
                        {
                            DesiredQuantity -= item.Quantity;
                            Target.Add(item);
                        }
                        else
                        {
                            Target.Add(item, DesiredQuantity);
                            return true;
                        }
                    }
                }
                else
                {
                    CurrentCargoAction.Target().Items.Where(CurrentCargoAction.QueryString).MoveTo(CurrentCargoAction.Source());
                }
            }
            catch { }
            return true;
        }

        bool NoOp(object[] Params)
        {
            return true;
        }

        #endregion

    }

}

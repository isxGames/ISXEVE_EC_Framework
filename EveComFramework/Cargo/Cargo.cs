using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EveCom;


namespace EveComFramework.Cargo
{

    public class Cargo : EveComFramework.Core.State
    {
        private class CargoAction
        {
            internal Func<object[], bool> Action { get; set; }
            internal Bookmark Bookmark { get; set; }
            internal string ContainerName { get; set; }
            internal Func<Item, bool> QueryString { get; set; }
            internal int Quantity { get; set; }
            internal Func<InventoryContainer> Source { get; set; }
            internal Func<InventoryContainer> Target { get; set; }

            internal CargoAction(Func<object[], bool> Action, Bookmark Bookmark, Func<InventoryContainer> Source, string ContainerName, Func<Item, bool> QueryString, int Quantity, Func<InventoryContainer> Target)
            {
                this.Action = Action;
                this.Bookmark = Bookmark;
                this.Source = Source;
                this.ContainerName = ContainerName;
                this.QueryString = QueryString;
                this.Quantity = Quantity;
                this.Target = Target;
            }

            public CargoAction Clone()
            {
                return new CargoAction(Action, Bookmark, Source, ContainerName, QueryString, Quantity, Target);
            }
        }

        #region Variables

        LinkedList<CargoAction> CargoQueue = new LinkedList<CargoAction>();
        CargoAction CurrentCargoAction;
        CargoAction BuildCargoAction;
        Move.Move Move = EveComFramework.Move.Move.Instance;

        #endregion

        #region Instantiation

        static Cargo _Instance;
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

        private Cargo() : base()
        {

        }

        #endregion

        #region Actions

        public CargoProxy At(Bookmark Bookmark, Func<InventoryContainer> Source = null, string ContainerName = "")
        {
            BuildCargoAction = new CargoAction(null, Bookmark, Source ?? (() => Station.ItemHangar), ContainerName, null, 0, null);
            return new CargoProxy();
        }

        public CargoProxy Load(Func<Item, bool> QueryString = null, int Quantity = 0, Func<InventoryContainer> Target = null)
        {
            BuildCargoAction.Action = Load;
            BuildCargoAction.QueryString = QueryString ?? (item => true);
            BuildCargoAction.Quantity = Quantity;
            BuildCargoAction.Target = Target ?? (() => MyShip.CargoBay);
            CargoQueue.AddFirst(BuildCargoAction.Clone());
            QueueState(Process);
            return new CargoProxy();
        }

        public CargoProxy Unload(Func<Item, bool> QueryString = null, int Quantity = 0, Func<InventoryContainer> Target = null)
        {
            BuildCargoAction.Action = Unload;
            BuildCargoAction.QueryString = QueryString ?? (item => true);
            BuildCargoAction.Quantity = Quantity;
            BuildCargoAction.Target = Target ?? (() => MyShip.CargoBay);
            CargoQueue.AddFirst(BuildCargoAction.Clone());
            QueueState(Process);
            return new CargoProxy();
        }

        public CargoProxy NoOp()
        {
            BuildCargoAction.Action = NoOp;
            BuildCargoAction.QueryString = null;
            BuildCargoAction.Quantity = 0;
            BuildCargoAction.Target = null;
            CargoQueue.AddFirst(BuildCargoAction.Clone());
            QueueState(Process);
            return new CargoProxy();
        }

        #endregion

        #region States

        bool Process(object[] Params)
        {
            if (CargoQueue.Count > 0)
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

            QueueState(Traveling, 5000);
            QueueState(WarpFleetMember);
            QueueState(Traveling, 3000);
            QueueState(CurrentCargoAction.Action);
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
            try
            {
                CurrentCargoAction.Source().Items.Where(CurrentCargoAction.QueryString).MoveTo(CurrentCargoAction.Target());
            }
            catch { }
            return true;
        }

        bool Unload(object[] Params)
        {
            try
            {
                CurrentCargoAction.Target().Items.Where(CurrentCargoAction.QueryString).MoveTo(CurrentCargoAction.Source());
            }
            catch { }
            return true;
        }

        bool NoOp(object[] Params)
        {
            return true;
        }

        #endregion

        #region Cargo move methods

        public class CargoProxy
        {

            public CargoProxy At(Bookmark Bookmark, Func<InventoryContainer> Source = null, string ContainerName = "")
            {
                Cargo.Instance.At(Bookmark, Source, ContainerName);
                return this;
            }

            public CargoProxy Load(Func<Item, bool> QueryString = null, int Quantity = 0, Func<InventoryContainer> Target = null)
            {
                Cargo.Instance.Load(QueryString, Quantity, Target);
                return this;
            }

            public CargoProxy Unload(Func<Item, bool> QueryString = null, int Quantity = 0, Func<InventoryContainer> Target = null)
            {
                Cargo.Instance.Unload(QueryString, Quantity, Target);
                return this;
            }

            public CargoProxy NoOp()
            {
                Cargo.Instance.NoOp();
                return this;
            }
        }

        #endregion


    }

}

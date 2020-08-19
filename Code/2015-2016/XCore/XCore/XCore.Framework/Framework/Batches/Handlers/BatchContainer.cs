//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reactive.Concurrency;
//using System.Reactive.Subjects;
//using System.Text;
//using System.Threading.Tasks;
//using XCore.Framework.Framework.Batches.Contracts;

//namespace XCore.Framework.Framework.Batches.Handlers
//{
//    public class BatchContainer<TEntityType>
//    {
//        #region props.

//        private ReplaySubject<TEntityType> Stream { get; set; }

//        #endregion
//        #region cst.

//        public BatchContainer( int maxSize , TimeSpan timeout )
//        {
//            this.Stream = new ReplaySubject<TEntityType>( maxSize , timeout , NewThreadScheduler.Default );
//        }

//        #endregion
//        #region publics.

//        public void Append( TEntityType entity )
//        {
//            this.Stream.OnNext( entity );
//        }
//        public void Subscribe( IBatchProcessor<TEntityType> processor )
//        {
//            this.Stream.Subscribe( entity => { processor.Process( entity ); } );
//        }

//        #endregion
//        #region helpers.

//        #endregion
//    }
//}

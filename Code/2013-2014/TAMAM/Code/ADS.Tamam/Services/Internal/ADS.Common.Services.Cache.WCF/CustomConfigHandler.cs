using System;
using System.Xml;
using System.Configuration;
using System.Collections.Generic;

using ADS.Common.Utilities;
using ADS.Common.Handlers.Cache;

namespace ADS.Common.Services.Cache.WCF
{
    public class CustomConfigHandler : IConfigurationSectionHandler
    {
        public object Create( object parent , object configContext , XmlNode section )
        {
            try
            {
                return GetClusters( section );
            }
            catch ( Exception x )
            {
                XLogger.Error( "Wrong Configuration Provided ..." );
                XLogger.Error( "Exception : " + x );
                return null;
            }
        }

        private Dictionary<Guid , CacheEngine.Cluster> GetClusters( XmlNode xmlRoot )
        {
            #region LOG
            XLogger.Info( "Start Importing Clusters from Configuration ..." );
            #endregion

            var clusters = new Dictionary<Guid , CacheEngine.Cluster>();

            // get clusters ( without dependencies ) ...
            foreach ( var xmlNode in xmlRoot.ChildNodes )
            {
                var node = ( XmlNode ) xmlNode;
                var id = node.Attributes["Id"].InnerText;
                var absoluteExpiration = node.Attributes["AbsoluteExpiration"] == null ? "" : node.Attributes["AbsoluteExpiration"].InnerText;

                CacheEngine.Cluster cluster;
                if ( string.IsNullOrWhiteSpace( absoluteExpiration ) )
                {
                    cluster = new CacheEngine.Cluster( Guid.Parse( id ) );
                }
                else
                {
                    var absoluteExpirationTimeSpan = TimeSpan.Parse( absoluteExpiration );
                    cluster = new CacheEngine.Cluster( Guid.Parse( id ), absoluteExpirationTimeSpan );
                }

                clusters.Add( cluster.Id , cluster );
            }

            #region LOG
            XLogger.Info( "Clusters Found : " + clusters.Keys.Count );
            #endregion

            // get dependencies ...
            foreach ( var xmlNode in xmlRoot.ChildNodes )
            {
                var node = ( XmlNode ) xmlNode;
                if ( node.HasChildNodes )
                {
                    var dependencies = new List<CacheEngine.Cluster>();

                    foreach ( var xmlChildNode in node.ChildNodes )
                    {
                        var dependencyId = ( ( XmlNode ) xmlChildNode ).Attributes["Id"].InnerText;
                        dependencies.Add( clusters[Guid.Parse( dependencyId )] );
                    }

                    var id = node.Attributes["Id"].InnerText;
                    var cluster = clusters[Guid.Parse( id )];
                    cluster.Parents = dependencies;
                }
            }

            #region LOG
            XLogger.Info( "Clusters inporting is finished.");
            #endregion

            // ...
            return clusters;
        }
    }
}

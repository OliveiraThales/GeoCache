using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeoCache.Contracts
{
    public interface INoSqlRepository : IDisposable, IRepository
    {
        /// <summary>
        /// Insert a document in the data repository.
        /// </summary>
        /// <param name="featureClassName">featureClassName</param>
        /// <param name="geometry">
        /// O documento
        /// </param>
        void Insert(string featureClassName, IGeometry geometry);

        /// <summary>
        /// Remove all the data from the repository.
        /// </summary>
        void RemoveAll(string featureClassName);
    }
}
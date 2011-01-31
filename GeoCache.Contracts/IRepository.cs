using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeoCache.Contracts
{
    public interface IRepository : IDisposable
    {
        /// <summary>
        /// Insert a document in the data repository.
        /// </summary>
        /// <param name="document">
        /// O documento
        /// </param>
        void Insert(Dictionary<string, object> document);

        /// <summary>
        /// Remove all the data from the repository.
        /// </summary>
        void RemoveAll();

        /// <summary>
        /// Gets all the documents from the repository.
        /// </summary>
        IEnumerable<Dictionary<string, object>> GetAll();

        /// <summary>
        /// Updates a document in the data repository.
        /// </summary>
        void Update(Dictionary<string, object> queryDocument, Dictionary<string, object> updatingDocument);
    }
}
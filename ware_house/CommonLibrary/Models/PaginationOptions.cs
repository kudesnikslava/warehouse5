using CommonLibrary.Enums;

namespace CommonLibrary.Models
{
    public class PaginationOptions
    {
        /// <summary>
        /// Offset
        /// </summary>
        public int? Offset { get; set; }
        
        /// <summary>
        /// Limit
        /// </summary>
        public int? Limit { get; set; }
        
        /// <summary>
        /// Ordering
        /// </summary>
        public Ordering Ordering { get; set; }
        
        /// <summary>
        /// Order by
        /// </summary>
        public string OrderBy { get; set; }
    }
}
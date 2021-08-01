using System;

namespace UnofficialDevryIT.Architecture.Models
{
    /// <summary>
    /// For tracking purposes of who modified / created what and when
    /// </summary>
    public abstract class AuditableEntity
    {
        /// <summary>
        /// Date in which record was created
        /// </summary>
        public DateTimeOffset Created { get; set; }
        
        /// <summary>
        /// Date in which record was last modified (if applicable)
        /// </summary>
        public DateTimeOffset? LastModified { get; set; }
        
        /// <summary>
        /// ID of user who last modified this record
        /// </summary>
        public string LastModifiedBy { get; set; }
        
        /// <summary>
        /// ID of user who created this record
        /// </summary>
        public string CreatedBy { get; set; }
    }
}
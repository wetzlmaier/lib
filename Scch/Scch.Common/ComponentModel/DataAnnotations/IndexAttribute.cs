using System;

namespace Scch.Common.ComponentModel.DataAnnotations
{
	/// <summary>
	/// Used to specify an index for a column
	/// </summary>
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
	public class IndexAttribute : Attribute
	{
		/// <summary>
		/// Position of the column in an index
		/// </summary>
		public int OrdinalPoistion { get; set; }

		/// <summary>
		/// Direction of the column sorting in an index
		/// </summary>
		public IndexDirection Direction { get; set; }

		/// <summary>
		/// Name of the index
		/// </summary>
		public string IndexName { get; set; }

        /// <summary>
        /// Uniqueness of the index.
        /// </summary>
        public bool IsUnique { get; set; }
    }
}

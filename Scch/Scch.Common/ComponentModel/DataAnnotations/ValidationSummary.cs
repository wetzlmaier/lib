using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Scch.Common.ComponentModel.DataAnnotations
{
    /// <summary>
    /// Summarizes the collection of <see cref="ValidationResult"/>.
    /// </summary>
    public class ValidationSummary : IEnumerable<ValidationResult>
    {
        private readonly List<ValidationResult> _results;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationSummary"/> class.
        /// </summary>
        public ValidationSummary()
        {
            _results = new List<ValidationResult>();
        }

        /// <summary>
        /// Adds a <see cref="ValidationResult"/>.
        /// </summary>
        /// <param name="result"></param>
        public void Add(ValidationResult result)
        {
            _results.Add(result);
        }

        /// <summary>
        /// Merges with another <see cref="ValidationSummary"/>.
        /// </summary>
        /// <param name="summary"></param>
        public void Merge(ValidationSummary summary)
        {
            _results.AddRange(summary._results);
        }

        /// <summary>
        /// Concatenates the <see cref="ValidationResult.ErrorMessage"/>.
        /// </summary>
        public string ErrorMessages
        {
            get
            {
                if (_results.Count == 0)
                    return string.Empty;

                var errorMessages = new StringBuilder();

                for (int i = 0; i < _results.Count; i++)
                    errorMessages.Append(Environment.NewLine + (i + 1) + ") " + _results[i].ErrorMessage);

                return errorMessages.ToString();
            }
        }

        /// <summary>
        /// Returns true, if the validation was successful.
        /// </summary>
        public bool IsValid
        {
            get { return _results.Count == 0; }
        }

        /// <summary>
        /// <see cref="object.ToString"/>
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ErrorMessages;
        }

        /// <summary>
        /// <see cref="IEnumerable{T}.GetEnumerator"/>
        /// </summary>
        /// <returns></returns>
        public IEnumerator<ValidationResult> GetEnumerator()
        {
            return _results.GetEnumerator();
        }

        /// <summary>
        /// <see cref="IEnumerable.GetEnumerator"/>
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.Serialization;
using System.Security;

namespace System
{
	/// <summary>Represents one or more errors that occur during application execution.</summary>
	/// <remarks>
	/// <see cref="T:System.AggregateException" /> is used to consolidate multiple failures into a single, throwable
	/// exception object.
	/// </remarks>
	[Serializable]
	[DebuggerDisplay("Count = {InnerExceptions.Count}")]
	public class AggregateException : Exception
	{
		private ReadOnlyCollection<Exception> m_innerExceptions;

		/// <summary>
		/// Gets a read-only collection of the <see cref="T:System.Exception" /> instances that caused the
		/// current exception.
		/// </summary>
		public ReadOnlyCollection<Exception> InnerExceptions => m_innerExceptions;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:System.AggregateException" /> class.
		/// </summary>
		public AggregateException()
			: base(Environment2.GetResourceString("AggregateException_ctor_DefaultMessage"))
		{
			m_innerExceptions = new ReadOnlyCollection<Exception>(new Exception[0]);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:System.AggregateException" /> class with
		/// a specified error message.
		/// </summary>
		/// <param name="message">The error message that explains the reason for the exception.</param>
		public AggregateException(string message)
			: base(message)
		{
			m_innerExceptions = new ReadOnlyCollection<Exception>(new Exception[0]);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:System.AggregateException" /> class with a specified error
		/// message and a reference to the inner exception that is the cause of this exception.
		/// </summary>
		/// <param name="message">The error message that explains the reason for the exception.</param>
		/// <param name="innerException">The exception that is the cause of the current exception.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="innerException" /> argument
		/// is null.</exception>
		public AggregateException(string message, Exception innerException)
			: base(message, innerException)
		{
			if (innerException == null)
			{
				throw new ArgumentNullException("innerException");
			}
			m_innerExceptions = new ReadOnlyCollection<Exception>(new Exception[1] { innerException });
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:System.AggregateException" /> class with
		/// references to the inner exceptions that are the cause of this exception.
		/// </summary>
		/// <param name="innerExceptions">The exceptions that are the cause of the current exception.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="innerExceptions" /> argument
		/// is null.</exception>
		/// <exception cref="T:System.ArgumentException">An element of <paramref name="innerExceptions" /> is
		/// null.</exception>
		public AggregateException(IEnumerable<Exception> innerExceptions)
			: this(Environment2.GetResourceString("AggregateException_ctor_DefaultMessage"), innerExceptions)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:System.AggregateException" /> class with
		/// references to the inner exceptions that are the cause of this exception.
		/// </summary>
		/// <param name="innerExceptions">The exceptions that are the cause of the current exception.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="innerExceptions" /> argument
		/// is null.</exception>
		/// <exception cref="T:System.ArgumentException">An element of <paramref name="innerExceptions" /> is
		/// null.</exception>
		public AggregateException(params Exception[] innerExceptions)
			: this(Environment2.GetResourceString("AggregateException_ctor_DefaultMessage"), innerExceptions)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:System.AggregateException" /> class with a specified error
		/// message and references to the inner exceptions that are the cause of this exception.
		/// </summary>
		/// <param name="message">The error message that explains the reason for the exception.</param>
		/// <param name="innerExceptions">The exceptions that are the cause of the current exception.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="innerExceptions" /> argument
		/// is null.</exception>
		/// <exception cref="T:System.ArgumentException">An element of <paramref name="innerExceptions" /> is
		/// null.</exception>
		public AggregateException(string message, IEnumerable<Exception> innerExceptions)
			: this(message, (innerExceptions == null) ? null : new List<Exception>(innerExceptions))
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:System.AggregateException" /> class with a specified error
		/// message and references to the inner exceptions that are the cause of this exception.
		/// </summary>
		/// <param name="message">The error message that explains the reason for the exception.</param>
		/// <param name="innerExceptions">The exceptions that are the cause of the current exception.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="innerExceptions" /> argument
		/// is null.</exception>
		/// <exception cref="T:System.ArgumentException">An element of <paramref name="innerExceptions" /> is
		/// null.</exception>
		public AggregateException(string message, params Exception[] innerExceptions)
			: this(message, (IList<Exception>)innerExceptions)
		{
		}

		/// <summary>
		/// Allocates a new aggregate exception with the specified message and list of inner exceptions.
		/// </summary>
		/// <param name="message">The error message that explains the reason for the exception.</param>
		/// <param name="innerExceptions">The exceptions that are the cause of the current exception.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="innerExceptions" /> argument
		/// is null.</exception>
		/// <exception cref="T:System.ArgumentException">An element of <paramref name="innerExceptions" /> is
		/// null.</exception>
		private AggregateException(string message, IList<Exception> innerExceptions)
			: base(message, (innerExceptions != null && innerExceptions.Count > 0) ? innerExceptions[0] : null)
		{
			if (innerExceptions == null)
			{
				throw new ArgumentNullException("innerExceptions");
			}
			Exception[] array = new Exception[innerExceptions.Count];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = innerExceptions[i];
				if (array[i] == null)
				{
					throw new ArgumentException(Environment2.GetResourceString("AggregateException_ctor_InnerExceptionNull"));
				}
			}
			m_innerExceptions = new ReadOnlyCollection<Exception>(array);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:System.AggregateException" /> class with serialized data.
		/// </summary>
		/// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds
		/// the serialized object data about the exception being thrown.</param>
		/// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that
		/// contains contextual information about the source or destination. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="info" /> argument is null.</exception>
		/// <exception cref="T:System.Runtime.Serialization.SerializationException">The exception could not be deserialized correctly.</exception>
		[SecurityCritical]
		protected AggregateException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			Exception[] array = info.GetValue("InnerExceptions", typeof(Exception[])) as Exception[];
			if (array == null)
			{
				throw new SerializationException(Environment2.GetResourceString("AggregateException_DeserializationFailure"));
			}
			m_innerExceptions = new ReadOnlyCollection<Exception>(array);
		}

		/// <summary>
		/// Sets the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> with information about
		/// the exception.
		/// </summary>
		/// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds
		/// the serialized object data about the exception being thrown.</param>
		/// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that
		/// contains contextual information about the source or destination. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="info" /> argument is null.</exception>
		[SecurityCritical]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			base.GetObjectData(info, context);
			Exception[] array = new Exception[m_innerExceptions.Count];
			m_innerExceptions.CopyTo(array, 0);
			info.AddValue("InnerExceptions", array, typeof(Exception[]));
		}

		/// <summary>
		/// Returns the <see cref="T:System.AggregateException" /> that is the root cause of this exception.
		/// </summary>
		public override Exception GetBaseException()
		{
			Exception ex = this;
			AggregateException ex2 = this;
			while (ex2 != null && ex2.InnerExceptions.Count == 1)
			{
				ex = ex.InnerException;
				ex2 = ex as AggregateException;
			}
			return ex;
		}

		/// <summary>
		/// Invokes a handler on each <see cref="T:System.Exception" /> contained by this <see cref="T:System.AggregateException" />.
		/// </summary>
		/// <param name="predicate">The predicate to execute for each exception. The predicate accepts as an
		/// argument the <see cref="T:System.Exception" /> to be processed and returns a Boolean to indicate
		/// whether the exception was handled.</param>
		/// <remarks>
		/// Each invocation of the <paramref name="predicate" /> returns true or false to indicate whether the
		/// <see cref="T:System.Exception" /> was handled. After all invocations, if any exceptions went
		/// unhandled, all unhandled exceptions will be put into a new <see cref="T:System.AggregateException" />
		/// which will be thrown. Otherwise, the <see cref="M:System.AggregateException.Handle(System.Func{System.Exception,System.Boolean})" /> method simply returns. If any
		/// invocations of the <paramref name="predicate" /> throws an exception, it will halt the processing
		/// of any more exceptions and immediately propagate the thrown exception as-is.
		/// </remarks>
		/// <exception cref="T:System.AggregateException">An exception contained by this <see cref="T:System.AggregateException" /> was not handled.</exception>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="predicate" /> argument is
		/// null.</exception>
		public void Handle(Func<Exception, bool> predicate)
		{
			if (predicate == null)
			{
				throw new ArgumentNullException("predicate");
			}
			List<Exception> list = null;
			for (int i = 0; i < m_innerExceptions.Count; i++)
			{
				if (!predicate(m_innerExceptions[i]))
				{
					if (list == null)
					{
						list = new List<Exception>();
					}
					list.Add(m_innerExceptions[i]);
				}
			}
			if (list != null)
			{
				throw new AggregateException(Message, list);
			}
		}

		/// <summary>
		/// Flattens an <see cref="T:System.AggregateException" /> instances into a single, new instance.
		/// </summary>
		/// <returns>A new, flattened <see cref="T:System.AggregateException" />.</returns>
		/// <remarks>
		/// If any inner exceptions are themselves instances of
		/// <see cref="T:System.AggregateException" />, this method will recursively flatten all of them. The
		/// inner exceptions returned in the new <see cref="T:System.AggregateException" />
		/// will be the union of all of the the inner exceptions from exception tree rooted at the provided
		/// <see cref="T:System.AggregateException" /> instance.
		/// </remarks>
		public AggregateException Flatten()
		{
			List<Exception> list = new List<Exception>();
			List<AggregateException> list2 = new List<AggregateException>();
			list2.Add(this);
			int num = 0;
			while (list2.Count > num)
			{
				IList<Exception> innerExceptions = list2[num++].InnerExceptions;
				for (int i = 0; i < innerExceptions.Count; i++)
				{
					Exception ex = innerExceptions[i];
					if (ex != null)
					{
						AggregateException ex2 = ex as AggregateException;
						if (ex2 != null)
						{
							list2.Add(ex2);
						}
						else
						{
							list.Add(ex);
						}
					}
				}
			}
			return new AggregateException(Message, list);
		}

		/// <summary>
		/// Creates and returns a string representation of the current <see cref="T:System.AggregateException" />.
		/// </summary>
		/// <returns>A string representation of the current exception.</returns>
		public override string ToString()
		{
			string text = base.ToString();
			for (int i = 0; i < m_innerExceptions.Count; i++)
			{
				text = string.Format(CultureInfo.InvariantCulture, Environment2.GetResourceString("AggregateException_ToString"), text, Environment.NewLine, i, m_innerExceptions[i].ToString(), "<---", Environment.NewLine);
			}
			return text;
		}
	}
}

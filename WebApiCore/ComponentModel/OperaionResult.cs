using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace WebApiCore.ComponentModel
{
	public class OperationResult<TError, TModel>
		where TError : OperationError
	{
		protected List<TError> _errors;

		[System.Text.Json.Serialization.JsonIgnore]
		[Newtonsoft.Json.JsonIgnore]
		public bool Succeeded { get; protected set; }

		[JsonPropertyName("errors")]
		[JsonProperty("errors")]
		public IEnumerable<TError> Errors
		{
			get => _errors ?? (_errors = new List<TError>());
			set => _errors = new List<TError>(value);
		}

		[JsonPropertyName("result")]
		[JsonProperty("result")]
		public TModel Result { get; protected set; }

		protected OperationResult()
		{ }

		public static OperationResult<TError, TModel> Success(TModel result)
		{
			return new OperationResult<TError, TModel>
			{
				Result = result,
				Succeeded = true
			};
		}

		public static OperationResult<TError, TModel> Success(TModel result, IEnumerable<TError> errors)
        {
			return new OperationResult<TError, TModel>()
			{
				Result = result,
				Succeeded = true,
				Errors = errors
			};
        }

		public static OperationResult<TError, TModel> Failed(TError error)
		{
			return new OperationResult<TError, TModel>
			{
				Result = default,
				Succeeded = false,
				_errors = new List<TError>() { error }
			};
		}

		public static OperationResult<TError, TModel> Failed(IEnumerable<TError> errors)
		{
			return new OperationResult<TError, TModel>
			{
				Result = default,
				Succeeded = false,
				_errors = errors.ToList()
			};
		}
	}

	public class OperationResult<TModel> : OperationResult<OperationError, TModel>
	{
		protected OperationResult() : base()
		{ }

		public new static OperationResult<TModel> Success(TModel result)
		{
			return new OperationResult<TModel>
			{
				Result = result
			};
		}

		public new static OperationResult<TModel> Failed(OperationError error)
		{
			return new OperationResult<TModel>
			{
				Result = default,
				_errors = new List<OperationError>() { error }
			};
		}

		public new static OperationResult<TModel> Failed(IEnumerable<OperationError> errors)
		{
			return new OperationResult<TModel>
			{
				Result = default,
				_errors = errors.ToList()
			};
		}

		public static OperationResult<TModel> Failed(int errorCode, string errorMessage)
		{
			return new OperationResult<TModel>
			{
				Result = default,
				_errors = new List<OperationError>() { new OperationError(errorCode, errorMessage) }
			};
		}

		public static implicit operator OperationResult<TModel>(TModel result)
		{
			return OperationResult<TModel>.Success(result);
		}

		public static implicit operator OperationResult<TModel>(AggregateException exceptions)
		{
			if (exceptions.InnerExceptions != null && exceptions.InnerExceptions.Any())
			{
				return OperationResult<TModel>.Failed(exceptions.InnerExceptions
					.Select(ex => new OperationError(exceptions.HResult, string.IsNullOrEmpty(ex.Message) ? ex.GetType().Name : ex.Message)));
			}
			else
				return OperationResult<TModel>.Failed(exceptions.HResult, string.IsNullOrEmpty(exceptions.Message) ? exceptions.GetType().Name : exceptions.Message);
		}

		public static implicit operator OperationResult<TModel>(Exception exception)
		{
			return OperationResult<TModel>.Failed(exception.HResult, string.IsNullOrEmpty(exception.Message) ? exception.GetType().Name : exception.Message);
		}
	}

	public class OperationResult : OperationResult<string>
	{
		public static OperationResult Success()
		{
			return new OperationResult()
			{
				Result = "ok"
			};
		}
	}
}

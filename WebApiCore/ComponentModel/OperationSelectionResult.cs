using System.Collections.Generic;

namespace WebApiCore.ComponentModel
{
    public class OperationSelectionResult<TError, TEnumModel> : OperationResult<TError, SelectionEnumerable<TEnumModel>>
        where TError : OperationError
    {
        protected OperationSelectionResult()
        { }

        public static OperationSelectionResult<TError, TEnumModel> Success(IEnumerable<TEnumModel> result)
        {
            return new OperationSelectionResult<TError, TEnumModel>()
            {
                Result = new SelectionEnumerable<TEnumModel>(result),
                Succeeded = true
            };
        }

        public static OperationSelectionResult<TError, TEnumModel> Success(IEnumerable<TEnumModel> result, int totalCount)
        {
            return new OperationSelectionResult<TError, TEnumModel>()
            {
                Result = new SelectionEnumerable<TEnumModel>(result, totalCount),
                Succeeded = true
            };
        }
    }

    public class OperationSelectionResult<TEnumModel> : OperationSelectionResult<OperationError, TEnumModel>
    {
        protected OperationSelectionResult() : base()
        { }

        public static implicit operator OperationSelectionResult<TEnumModel>(TEnumModel[] result)
        {
            return new OperationSelectionResult<TEnumModel>()
            {
                Result = new SelectionEnumerable<TEnumModel>(result)
            };
        }

        public static implicit operator OperationSelectionResult<TEnumModel>(List<TEnumModel> result)
        {
            return new OperationSelectionResult<TEnumModel>()
            {
                Result = new SelectionEnumerable<TEnumModel>(result)
            };
        }
    }
}

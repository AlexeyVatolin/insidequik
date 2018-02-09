using System.Threading.Tasks;

namespace Common.Helpers
{
    //TODO: стоит избавиться от этого кода
    public static class TaskHelper
    {
        public static Task Empty { get; } = MakeTask((object)null);

        public static Task<TInput> MakeTask<TInput>(TInput value)
        {
            return FromResult(value);
        }

        public static Task<TInput> FromResult<TInput>(TInput value)
        {
            var completionSource = new TaskCompletionSource<TInput>();
            var result = value;
            completionSource.SetResult(result);
            return completionSource.Task;
        }
    }
}

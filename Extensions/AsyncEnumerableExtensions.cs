using TheOmenDen.Shared.Models;

namespace TheOmenDen.Shared.Extensions;

public static class AsynchronousStreamOutcome
{
    /// <summary>
    /// <para>Iterates through the provided <paramref name="source"/> stream, and enacts the provided <paramref name="outcomeGeneratorFunction"/> over each individual member <typeparamref name="T"/></para>
    /// <para>This allows for a retrieval of each individual item, and examination of the outcome of the enacted <paramref name="outcomeGeneratorFunction"/> upon each item</para>
    /// </summary>
    /// <typeparam name="T">The underlying type that we're operating over</typeparam>
    /// <param name="source">The source <see cref="IAsyncEnumerable{T}"/></param>
    /// <param name="outcomeGeneratorFunction">A given asynchronous function that we invoke upon a single <typeparamref name="T"/> during stream iteration</param>
    /// <param name="cancellationToken"><inheritdoc cref="CancellationToken"/></param>
    /// <returns>A transformed asynchronous stream, returning only each <see cref="OperationOutcome"/></returns>
    public static async IAsyncEnumerable<OperationOutcome> GetStreamOutcomesAsync<T>(this IAsyncEnumerable<T> source, Func<T, CancellationToken, ValueTask<OperationOutcome>> outcomeGeneratorFunction, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        where T : IEntity
    {
        await foreach (var item in source.WithCancellation(cancellationToken).ConfigureAwait(false))
        {
            var outcome = await outcomeGeneratorFunction.Invoke(item, cancellationToken).ConfigureAwait(false);

            if (outcome != OperationOutcome.SuccessfulOutcome)
            {
                var reportedException = new InvalidOperationException($"{nameof(item)} could not be processed successfully in the streaming context. Check {nameof(outcome)} for more details");

                outcome.ClientErrorPayload = new ClientErrorPayload<T>
                {
                    Data = item,
                    Message = reportedException.Message,
                    Detail = reportedException?.StackTrace
                             ?? reportedException?.Source
                        ?? $"{nameof(item)} threw an exception while iterating through the stream",
                    IsError = true
                };
            }

            yield return outcome;
        }
    }

    /// <summary>
    /// <para>Iterates through the provided <paramref name="source"/> stream, and enacts the provided <paramref name="outcomeGeneratorFunction"/> over each individual member <typeparamref name="T"/></para>
    /// <para>This allows for a retrieval of each individual item, and examination of the outcome of the enacted <paramref name="outcomeGeneratorFunction"/> upon each item</para>
    /// </summary>
    /// <typeparam name="T">The underlying type that we're operating over</typeparam>
    /// <param name="source">The source <see cref="IAsyncEnumerable{T}"/></param>
    /// <param name="outcomeGeneratorFunction">A given asynchronous function that we invoke upon a single <typeparamref name="T"/> during stream iteration</param>
    /// <param name="cancellationToken"><inheritdoc cref="CancellationToken"/></param>
    /// <returns>A transformed asynchronous stream, coupling the <typeparamref name="T"/> with the <see cref="OperationOutcome"/></returns>
    public static async IAsyncEnumerable<Tuple<T, OperationOutcome>> ReportStreamOutcomesAsync<T>(this IAsyncEnumerable<T> source, Func<T, CancellationToken, ValueTask<OperationOutcome>> outcomeGeneratorFunction, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        where T : IEntity
    {
        await foreach (var item in source.WithCancellation(cancellationToken).ConfigureAwait(false))
        {
            var outcome = await outcomeGeneratorFunction.Invoke(item, cancellationToken).ConfigureAwait(false);

            if (outcome != OperationOutcome.SuccessfulOutcome)
            {
                var reportedException = new InvalidOperationException($"{nameof(item)} could not be processed successfully in the streaming context. Check {nameof(outcome)} for more details");

                outcome.ClientErrorPayload = new ClientErrorPayload<T>
                {
                    Data = item,
                    Message = reportedException.Message,
                    Detail = reportedException?.StackTrace
                             ?? reportedException?.Source
                        ?? $"{nameof(item)} threw an exception while iterating through the stream",
                    IsError = true
                };
            }

            yield return new(item, outcome);
        }
    }

    /// <summary>
    /// <para>Iterates through the provided <paramref name="source"/> stream, and enacts the provided <paramref name="outcomeGeneratorFunction"/> over each individual member <typeparamref name="T"/></para>
    /// <para>This allows for a retrieval of each individual item, and examination of the outcome of the enacted <paramref name="outcomeGeneratorFunction"/> upon each item</para>
    /// </summary>
    /// <typeparam name="T">The underlying type that we're operating over</typeparam>
    /// <param name="source">The source <see cref="IAsyncEnumerable{T}"/></param>
    /// <param name="outcomeGeneratorFunction">A given asynchronous function that we invoke upon a single <typeparamref name="T"/> during stream iteration</param>
    /// <param name="cancellationToken"><inheritdoc cref="CancellationToken"/></param>
    /// <returns>A transformed asynchronous stream, coupling the <typeparamref name="T"/> with the <see cref="OperationOutcome"/></returns>
    public static async IAsyncEnumerable<Tuple<T, OperationOutcome>> ReportStreamOutcomesAsync<T>(this IAsyncEnumerable<T> source, Expression<Func<T, CancellationToken, ValueTask<OperationOutcome>>> outcomeGeneratorFunction, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        where T : IEntity
    {
        await foreach (var item in source
                           .WithCancellation(cancellationToken)
                           .ConfigureAwait(false))
        {
            var outcome = await outcomeGeneratorFunction
                .Compile()
                .Invoke(item, cancellationToken)
                .ConfigureAwait(false);

            if (outcome != OperationOutcome.SuccessfulOutcome)
            {
                var reportedException = new InvalidOperationException($"{nameof(item)} could not be processed successfully in the streaming context. Check {nameof(outcome)} for more details");

                outcome.ClientErrorPayload = new ClientErrorPayload<T>
                {
                    Data = item,
                    Message = reportedException.Message,
                    Detail = reportedException?.StackTrace
                             ?? reportedException?.Source
                        ?? $"{nameof(item)} threw an exception while iterating through the stream",
                    IsError = true
                };
            }

            yield return new(item, outcome);
        }
    }
}


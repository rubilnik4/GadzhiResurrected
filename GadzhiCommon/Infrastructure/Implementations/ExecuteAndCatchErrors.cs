﻿using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Models.Implementations.Errors;
using GadzhiCommon.Models.Interfaces.Errors;
using System;
using System.ComponentModel;
using System.ServiceModel;
using System.Threading.Tasks;
using GadzhiCommon.Extensions.Functional;
using GadzhiCommon.Models.Implementations.Functional;

namespace GadzhiCommon.Infrastructure.Implementations
{
    /// <summary>
    /// Класс обертка для отлова ошибок
    /// </summary> 
    public static class ExecuteAndCatchErrors
    {
        /// <summary>
        /// Отлов ошибок и вызов метода       
        /// </summary> 
        public static IResultError ExecuteAndHandleError(Action method, Action beforeMethod = null, Action catchMethod = null,
                                                         Action finallyMethod = null, IErrorCommon errorMessage = null) =>
            ExecuteAndHandleError(() => { method.Invoke(); return Unit.Value; },
                                  beforeMethod, catchMethod, finallyMethod, errorMessage).
            ToResult();

        /// <summary>
        /// Отлов ошибок и вызов метода       
        /// </summary> 
        public static IResultValue<T> ExecuteAndHandleError<T>(Func<T> method, Action beforeMethod = null, Action catchMethod = null,
                                                               Action finallyMethod = null, IErrorCommon errorMessage = null)
        {
            if (method == null) throw new ArgumentNullException(nameof(method));

            IResultValue<T> result;

            try
            {
                beforeMethod?.Invoke();
                result = new ResultValue<T>(method.Invoke());
            }
            catch (Exception exception)
            {
                catchMethod?.Invoke();
                result = new ErrorCommon(GetTypeException(exception), String.Empty, exception).
                         ToResultValue<T>().
                         ConcatErrors(errorMessage);
            }
            finally
            {
                finallyMethod?.Invoke();
            }

            return result;
        }

        /// <summary>
        /// Отлов ошибок и вызов метода асинхронно     
        /// </summary> 
        public static async Task<IResultError> ExecuteAndHandleErrorAsync(Func<Task> asyncMethod, Action beforeMethod = null,
                                                                          Action catchMethod = null, Action finallyMethod = null,
                                                                          IErrorCommon errorMessage = null) =>
            await ExecuteAndHandleErrorAsync(async () =>  { await asyncMethod.Invoke(); return Task.FromResult(Unit.Value); },
                                             beforeMethod, catchMethod, finallyMethod, errorMessage).
            MapAsync(result => result.ToResult());

        /// <summary>
        /// Отлов ошибок и вызов метода асинхронно
        /// </summary> 
        public static async Task<IResultValue<T>> ExecuteAndHandleErrorAsync<T>(Func<Task<T>> asyncFunc, Action beforeMethod = null, 
                                                                                Action catchMethod = null, Action finallyMethod = null, 
                                                                                IErrorCommon errorMessage = null)
        {
            if (asyncFunc == null) throw new ArgumentNullException(nameof(asyncFunc));

            IResultValue<T> result;

            try
            {
                beforeMethod?.Invoke();
                result = new ResultValue<T>(await asyncFunc.Invoke());
            }
            catch (Exception exception)
            {
                catchMethod?.Invoke();
                result = new ErrorCommon(GetTypeException(exception), String.Empty, exception).
                         ToResultValue<T>().
                         ConcatErrors(errorMessage);
            }
            finally
            {
                finallyMethod?.Invoke();
            }

            return result;
        }

        /// <summary>
        /// Получить тип ошибки
        /// </summary>       
        public static ErrorConvertingType GetTypeException(Exception exception) =>
            exception switch
            {
                NullReferenceException _ => ErrorConvertingType.NullReference,
                ArgumentNullException _ => ErrorConvertingType.ArgumentNullReference,
                FormatException _ => ErrorConvertingType.FormatException,
                InvalidEnumArgumentException _ => ErrorConvertingType.InvalidEnumArgumentException,
                TimeoutException _ => ErrorConvertingType.TimeOut,
                CommunicationException _ => ErrorConvertingType.Communication,
                _ => ErrorConvertingType.UnknownError
            };
    }
}

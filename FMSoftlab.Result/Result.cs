using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Headers;

namespace FMSoftlab.Result
{
    public enum MessageType { Success, Info, Warning, Error };
    public class MessageItem
    {
        public MessageType MessageType { get; set; }
        public int Code { get; set; }
        public string Message { get; set; }
        public MessageItem()
        {
            Message=string.Empty;
            Code=0;
            MessageType=MessageType.Info;
        }
    }
    public interface IResult
    {
        bool Success { get; set; }
        bool Failure { get; }
        List<MessageItem> Messages { get; }
    }

    public interface IResultMessages : IResult
    {
        void AddFailure(int code, string message);
        void AddFailure();
        void AddSuccess(int code, string message);
        void AddSuccess();

    }
    public class Result : IResult, IResultMessages
    {
        public bool Success { get; set; }
        public bool Failure { get { return !Success; } }
        public List<MessageItem> Messages { get; }

        public Result(bool success)
        {
            Messages=new List<MessageItem>();
            Success=success;
        }
        public Result()
        {
            Messages=new List<MessageItem>();
            Success=false;
        }
        public static Result FailureResult(int code, string message) => new FailureResult(code, message);
        public static Result FailureResult() => new FailureResult();
        public static Result SuccessResult<T>(T value) => new SuccessResult<T>(value);

        public void AddMessage(MessageItem messageItem)
        {
            Messages.Add(messageItem);
        }
        public void AddMessage(MessageType messageType, int code, string message)
        {
            AddMessage(new MessageItem { Message=message, MessageType=messageType, Code=code });
        }
        public void AddSuccess(int code, string message)
        {
            AddMessage(MessageType.Success, code, message);
        }
        public void AddSuccess()
        {
            AddMessage(MessageType.Success, 0, string.Empty);
        }
        public void AddFailure(int code, string message)
        {
            AddMessage(MessageType.Error, code, message);
        }
        public void AddFailure()
        {
            AddMessage(MessageType.Error, 0, string.Empty);
        }
    }

    public class Result<T> : Result
    {
        public T Value { get; set; }
        public Result() : base()
        {

        }
        public Result(bool success) : base(success)
        {

        }
    }

    public class SuccessResult<T> : Result<T>
    {
        public SuccessResult(T value) : base(true)
        {
            Value=value;
        }
    }
    public class FailureResult : Result
    {
        public FailureResult(int code, string message) : base(false)
        {
            AddFailure(code, message);
        }
        public FailureResult() : base(false)
        {

        }
        public FailureResult(MessageItem messageItem) : base(false)
        {
            AddMessage(new MessageItem { MessageType=MessageType.Error, Code=messageItem.Code, Message=messageItem.Message });
        }
    }
}
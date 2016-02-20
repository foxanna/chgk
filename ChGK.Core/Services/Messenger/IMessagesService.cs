using System;
using MvvmCross.Plugins.Messenger;

namespace ChGK.Core.Services.Messenger
{
    public interface IMessagesService
    {
        void Publish(MvxMessage message);

        object Subscribe<T>(Action<T> deliveryAction) where T : MvxMessage;
        object SubscribeOnMainThread<T>(Action<T> deliveryAction) where T : MvxMessage;

        void Unsubscribe<T>(object mvxSubscriptionId) where T : MvxMessage;
    }
}
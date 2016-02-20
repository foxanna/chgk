using System;
using MvvmCross.Plugins.Messenger;

namespace ChGK.Core.Services.Messenger
{
    internal class MessagesService : IMessagesService
    {
        private readonly IMvxMessenger _messenger;

        public MessagesService(IMvxMessenger messenger)
        {
            _messenger = messenger;
        }

        public void Publish(MvxMessage message)
        {
            _messenger.Publish(message);
        }

        public object Subscribe<T>(Action<T> deliveryAction) where T : MvxMessage
        {
            return _messenger.Subscribe(deliveryAction);
        }

        public object SubscribeOnMainThread<T>(Action<T> deliveryAction) where T : MvxMessage
        {
            return _messenger.SubscribeOnMainThread(deliveryAction);
        }

        public void Unsubscribe<T>(object mvxSubscriptionId) where T : MvxMessage
        {
            _messenger.Unsubscribe<T>(mvxSubscriptionId as MvxSubscriptionToken);
        }
    }
}
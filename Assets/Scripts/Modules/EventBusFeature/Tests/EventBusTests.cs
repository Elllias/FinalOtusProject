using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using Modules.EventBusFeature;

namespace Modules.EventBusFeature.Tests
{
    public class EventBusTests
    {
        private class TestEvent { }
        private class AnotherTestEvent { }

        private bool _eventRaised;
        private bool _anotherEventRaised;

        [SetUp]
        public void SetUp()
        {
            _eventRaised = false;
            _anotherEventRaised = false;
        }

        [Test]
        public void SubscribeTest()
        {
            EventBus.Subscribe<TestEvent>(OnTestEvent);

            EventBus.RaiseEvent(new TestEvent());
            
            EventBus.Unsubscribe<TestEvent>(OnTestEvent);

            Assert.IsTrue(_eventRaised);
        }

        [Test]
        public void UnsubscribeTest()
        {
            EventBus.Subscribe<TestEvent>(OnTestEvent);
            EventBus.Unsubscribe<TestEvent>(OnTestEvent);

            EventBus.RaiseEvent(new TestEvent());

            Assert.IsFalse(_eventRaised);
        }

        [Test]
        public void RaiseEventTest_NoSubscribers()
        {
            EventBus.RaiseEvent(new TestEvent());

            Assert.IsFalse(_eventRaised);
        }

        [Test]
        public void RaiseEventTest_MultipleSubscribers()
        {
            EventBus.Subscribe<TestEvent>(OnTestEvent);
            EventBus.Subscribe<TestEvent>(OnTestEvent);

            EventBus.RaiseEvent(new TestEvent());
            
            EventBus.Unsubscribe<TestEvent>(OnTestEvent);
            EventBus.Unsubscribe<TestEvent>(OnTestEvent);

            Assert.IsTrue(_eventRaised);
        }

        [Test]
        public void RaiseEventTest_DifferentEventTypes()
        {
            EventBus.Subscribe<TestEvent>(OnTestEvent);
            EventBus.Subscribe<AnotherTestEvent>(OnAnotherTestEvent);

            EventBus.RaiseEvent(new TestEvent());
            EventBus.RaiseEvent(new AnotherTestEvent());
            
            EventBus.Unsubscribe<TestEvent>(OnTestEvent);
            EventBus.Unsubscribe<AnotherTestEvent>(OnAnotherTestEvent);

            Assert.IsTrue(_eventRaised);
            Assert.IsTrue(_anotherEventRaised);
        }

        private void OnTestEvent(TestEvent evt)
        {
            _eventRaised = true;
        }

        private void OnAnotherTestEvent(AnotherTestEvent evt)
        {
            _anotherEventRaised = true;
        }
    }
}

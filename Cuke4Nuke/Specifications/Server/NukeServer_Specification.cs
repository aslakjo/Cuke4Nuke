using System.IO;
using System;

using Cuke4Nuke.Core;
using Cuke4Nuke.Server;
using Cuke4Nuke.Specifications.Core;

using NUnit.Framework;

namespace Cuke4Nuke.Specifications.Server
{
    [TestFixture]
    public class NukeServer_Specification
    {
        MockListener _listener;
        StringWriter _outputWriter;

        [SetUp]
        public void SetUp()
        {
            _listener = new MockListener();
            _outputWriter = new StringWriter();
        }

        [Test]
        public void Start_without_help_option_should_start_the_listener()
        {
            var server = new NukeServer(_listener, new Options());
            server.Start();

            Assert.That(_listener.HasMessageLoggedListeners());
            Assert.That(_listener.StartCalled);
            Assert.That(_listener.StopCalled);
        }

        [Test]
        public void Start_with_and_wait_for_a_debugger()
        {
            var mockDebuggerFacade = new MockDebugerFacade();
            var server = new NukeServer(_listener, new Options(), mockDebuggerFacade);
            
            server.WaitForDebugerAndRun();

            Assert.That(_listener.StartCalled);
            Assert.That(mockDebuggerFacade.HasWaitBeenCalled());
        }

        [Test]
        public void Start_with_minus_d_should_wait_for_a_debugger()
        {
            var mockDebugerFacade = new MockDebugerFacade();
            var server = new NukeServer(_listener, new Options("-d"), mockDebugerFacade);

            server.Start();

            Assert.That(_listener.StartCalled);
            Assert.That(mockDebugerFacade.HasWaitBeenCalled());
        }

        internal class MockDebugerFacade : IDebugerCommand
        {
            private bool _waitedForDebuger = false;

            public bool HasWaitBeenCalled()
            {
                return _waitedForDebuger;
            }

            public void WaitForDebuger()
            {
                _waitedForDebuger = true;
            }
        }

        class MockListener : Listener
        {
            internal bool StartCalled;
            internal bool StopCalled;

            public MockListener()
                : base(new Processor(new Loader(new System.Collections.Generic.List<string>(), null), null), 0)
            {
            }

            public override void Start()
            {
                StartCalled = true;
            }

            public override void Stop()
            {
                StopCalled = true;
            }

            internal bool HasMessageLoggedListeners()
            {
                return MessageLogged != null;
            }
        }
    }
}
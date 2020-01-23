using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Microsoft.Reactive.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GadzhiTest.Modules.FilesConvertModule.ViewModels
{
    [TestClass]
    public class UnitTest1 : ReactiveTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var scheduler = new TestScheduler();

            var input = scheduler.CreateHotObservable(
                OnNext(100, "abc"),
                OnNext(200, "def"),
                OnNext(250, "ghi"),
                OnNext(300, "pqr"),
                OnNext(450, "xyz"),
                OnCompleted<string>(500)
                );

            var results = scheduler.Start(
                () => input.Buffer(() => input.Throttle(TimeSpan.FromTicks(100), scheduler))
                           .Select(b => string.Join(",", b)),
                created: 50,
                subscribed: 150,
                disposed: 600);

            ReactiveAssert.AreElementsEqual(results.Messages, new Recorded<Notification<string>>[] {
                OnNext(400, "def,ghi,pqr"),
                OnNext(500, "xyz"),
                OnCompleted<string>(500)
            });

            ReactiveAssert.AreElementsEqual(input.Subscriptions, new Subscription[] {
                Subscribe(150, 500),
                Subscribe(150, 400),
                Subscribe(400, 500)
            });
        }
    }
}

using System;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Interfaces;
using System.Reactive.Linq;
using PlayGround.Contracts.Services.HelloWorld;
using PlayGround.Contracts.ViewModels;
using PlayGround.Models;
using ReactiveUI;

namespace PlayGround.ViewModels
{
    public class CoverViewModel : ViewModelBase, ICoverViewModel
	{
        private readonly string _title;
        private readonly string _imageUrl;
        private readonly IScheduler _backgroundScheduler;
        private readonly IHelloWorldService _helloWorldService;

        private ObservableAsPropertyHelper<string> _property1;
        private ObservableAsPropertyHelper<string> _property2;
        private ObservableAsPropertyHelper<string> _property3;
        private ObservableAsPropertyHelper<string> _property4;
        private ObservableAsPropertyHelper<string> _property5;
        private ObservableAsPropertyHelper<string> _property6;
        private ObservableAsPropertyHelper<string> _property7;

        //private string _property1;
        //private string _property2;
        //private string _property3;
        //private string _property4;
        //private string _property5;
        //private string _property6;
        //private string _property7;

		public CoverViewModel(
            Cover cover,
            IHelloWorldService helloWorldService,
            IScheduler backgroundScheduler)
		{
            _title = cover.Title;
            _imageUrl = cover.Links.ImageLink.Href;
            _backgroundScheduler = backgroundScheduler;
            _helloWorldService = helloWorldService;

            InitProperties();
		}

        private void InitProperties()
        {
            _helloWorldService
                .GetHelloWorld()
                .Delay(TimeSpan.FromSeconds(1), _backgroundScheduler)
                .Select(x => $"Property1 {x.Name}")
                .StartWith("Property1")
                .Take(1)
                .ToProperty(this, x => x.Property1, out _property1)
                //.Subscribe(x => _property1 = x)
                .DisposeWith(Disposables);

            _helloWorldService
                .GetHelloWorld()
                .Delay(TimeSpan.FromSeconds(1), _backgroundScheduler)
                .Select(x => $"Property2 {x.Name}")
                .StartWith("Property2")
                .Take(1)
                .ToProperty(this, x => x.Property2, out _property2)
                //.Subscribe(x => _property2 = x)
                .DisposeWith(Disposables);

            _helloWorldService
                .GetHelloWorld()
                .Delay(TimeSpan.FromSeconds(1), _backgroundScheduler)
                .Select(x => $"Property3 {x.Name}")
                .StartWith("Property3")
                .Take(1)
                .ToProperty(this, x => x.Property3, out _property3)
                //.Subscribe(x => _property3 = x)
                .DisposeWith(Disposables);

            _helloWorldService
                .GetHelloWorld()
                .Delay(TimeSpan.FromSeconds(1), _backgroundScheduler)
                .Select(x => $"Property4 {x.Name}")
                .StartWith("Property4")
                .Take(1)
                .ToProperty(this, x => x.Property4, out _property4)
                //.Subscribe(x => _property4 = x)
                .DisposeWith(Disposables);

            _helloWorldService
                .GetHelloWorld()
                .Delay(TimeSpan.FromSeconds(1), _backgroundScheduler)
                .Select(x => $"Property5 {x.Name}")
                .StartWith("Property5")
                .Take(1)
                .ToProperty(this, x => x.Property5, out _property5)
                //.Subscribe(x => _property5 = x)
                .DisposeWith(Disposables);

            _helloWorldService
                .GetHelloWorld()
                .Delay(TimeSpan.FromSeconds(1), _backgroundScheduler)
                .Select(x => $"Property6 {x.Name}")
                .StartWith("Property6")
                .Take(1)
                .ToProperty(this, x => x.Property6, out _property6)
                //.Subscribe(x => _property6 = x)
                .DisposeWith(Disposables);

            _helloWorldService
                .GetHelloWorld()
                .Delay(TimeSpan.FromSeconds(1), _backgroundScheduler)
                .Select(x => $"Property7 {x.Name}")
                .StartWith("Property7")
                .Take(1)
                .ToProperty(this, x => x.Property7, out _property7)
                //.Subscribe(x => _property7 = x)
                .DisposeWith(Disposables);

            //_testCounter
            //    .Delay(TimeSpan.FromSeconds(2))
            //    .Select(x => $"Property2 count:{x}")
            //    .StartWith("Property2")
            //    .ToProperty(this, x => x.Property2, out _property2)
            //    .DisposeWith(Disposables);

            //_testCounter
            //    .Delay(TimeSpan.FromSeconds(3))
            //    .Select(x => $"Property3 count:{x}")
            //    .StartWith("Property3")
            //    .ToProperty(this, x => x.Property3, out _property3)
            //    .DisposeWith(Disposables);

            //_testCounter
            //    .Delay(TimeSpan.FromSeconds(4))
            //    .Select(x => $"Property4 count:{x}")
            //    .StartWith("Property4")
            //    .ToProperty(this, x => x.Property4, out _property4)
            //    .DisposeWith(Disposables);

            //_testCounter
            //    .Delay(TimeSpan.FromSeconds(5))
            //    .Select(x => $"Property5 count:{x}")
            //    .StartWith("Property5")
            //    .ToProperty(this, x => x.Property5, out _property5)
            //    .DisposeWith(Disposables);

            //_testCounter
            //    .Delay(TimeSpan.FromSeconds(6))
            //    .Select(x => $"Property6 count:{x}")
            //    .StartWith("Property6")
            //    .ToProperty(this, x => x.Property6, out _property6)
            //    .DisposeWith(Disposables);

            //_testCounter
                //.Delay(TimeSpan.FromSeconds(7))
                //.Select(x => $"Property7 count:{x}")
                //.StartWith("Property7")
                //.ToProperty(this, x => x.Property7, out _property7)
                //.DisposeWith(Disposables);
        }

        protected override void InitLifeCycleAwareProperties(CompositeDisposable lifeCycleDisposable)
        {
        }

        public string Title => _title;
        public string ImageUrl => _imageUrl;

        public string Property1 => _property1.Value;
        public string Property2 => _property2.Value;
        public string Property3 => _property3.Value;
        public string Property4 => _property4.Value;
        public string Property5 => _property5.Value;
        public string Property6 => _property6.Value;
        public string Property7 => _property7.Value;

        //public string Property1 => _property1;
        //public string Property2 => _property2;
        //public string Property3 => _property3;
        //public string Property4 => _property4;
        //public string Property5 => _property5;
        //public string Property6 => _property6;
        //public string Property7 => _property7;

        //public string Property1 => "bla bli blub";
        //public string Property2 => "bla bli blub";
        //public string Property3 => "bla bli blub";
        //public string Property4 => "bla bli blub";
        //public string Property5 => "bla bli blub";
        //public string Property6 => "bla bli blub";
        //public string Property7 => "bla bli blub";
    }
}

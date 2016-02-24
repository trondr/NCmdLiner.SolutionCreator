using System;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Common.Logging;
using Common.Logging.Simple;
using NCmdLiner.SolutionCreator.Library.Common;
using NCmdLiner.SolutionCreator.Library.Services;
using NUnit.Framework;
using Rhino.Mocks;

namespace NCmdLiner.SolutionCreator.Library.Tests.UnitTests
{
    [TestFixture(Category = "UnitTests")]
    public class ContextTests
    {
        private ILog _logger;
        private IGuidGeneator _stubGuidGenerator;
        private string _guid1;
        private string _guid2;        

        [SetUp]
        public void SetUp()
        {
            _logger = new ConsoleOutLogger(this.GetType().Name, LogLevel.All, true, false, false, "yyyy-MM-dd hh:mm:ss");

            _guid1 = Guid.NewGuid().ToString(); 
            Console.WriteLine("Guid1="+_guid1);
            
            _guid2 = Guid.NewGuid().ToString(); 
            Console.WriteLine("Guid2=" + _guid2);            
            
        }

        [TearDown]
        public void TearDown()
        {

        }

        [Test]
        public void ContextAddVariableGetVariableTest()
        {
            _stubGuidGenerator = MockRepository.GenerateStub<IGuidGeneator>();
            _stubGuidGenerator.Stub(geneator => geneator.GetNewGuid()).Repeat.Never();

            var target = new ResolveContext(_stubGuidGenerator, _logger);
            const string variableName = "SomeVariableName";
            const string expected = "SomeValue";
            var actual = target.GetVariable(variableName);
            Assert.AreEqual(null, actual);
            target.AddVariable(variableName, expected);
            actual = target.GetVariable(variableName);
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void ContextGuidTest()
        {
            _stubGuidGenerator = MockRepository.GenerateStub<IGuidGeneator>();
            _stubGuidGenerator.Stub(geneator => geneator.GetNewGuid()).Return(_guid1).Repeat.Once();
            _stubGuidGenerator.Stub(geneator => geneator.GetNewGuid()).Return(_guid2).Repeat.Once();
            

            var target = new ResolveContext(_stubGuidGenerator, _logger);
            var actual = target.GetVariable("Guid1");
            var expected = _guid1;
            Assert.AreEqual(expected, actual);
            actual = target.GetVariable("Guid2");
            expected = _guid2;
            Assert.AreEqual(expected, actual);
            actual = target.GetVariable("Guid1");
            expected = _guid1;
            Assert.AreEqual(expected, actual);
            actual = target.GetVariable("Guid2");
            expected = _guid2;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ContextInvalidGuidNameTest()
        {
            _stubGuidGenerator = MockRepository.GenerateStub<IGuidGeneator>();
            _stubGuidGenerator.Stub(geneator => geneator.GetNewGuid()).Repeat.Never();
            
            var target = new ResolveContext(_stubGuidGenerator, _logger);
            var actual = target.GetVariable("Guidd1");
            string expected = null;
            Assert.AreEqual(expected, actual);
            actual = target.GetVariable("Guidd2");
            expected = null;
            Assert.AreEqual(expected, actual);
            actual = target.GetVariable("Guidd1");
            expected = null;
            Assert.AreEqual(expected, actual);
            actual = target.GetVariable("Guidd2");
            expected = null;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ContextInvalidLowerCaseGuidNameTest()
        {
            _stubGuidGenerator = MockRepository.GenerateStub<IGuidGeneator>();
            _stubGuidGenerator.Stub(geneator => geneator.GetNewGuid()).Repeat.Never();

            var target = new ResolveContext(_stubGuidGenerator, _logger);
            var actual = target.GetVariable("guid1");
            string expected = null;
            Assert.AreEqual(expected, actual);
            actual = target.GetVariable("guid2");
            expected = null;
            Assert.AreEqual(expected, actual);
            actual = target.GetVariable("guid1");
            expected = null;
            Assert.AreEqual(expected, actual);
            actual = target.GetVariable("guid2");
            expected = null;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ContextGetVariableSpecialGuidTest()
        {
            _stubGuidGenerator = MockRepository.GenerateStub<IGuidGeneator>();
            _stubGuidGenerator.Stub(geneator => geneator.GetNewGuid()).Return(_guid1).Repeat.Once();
            _stubGuidGenerator.Stub(geneator => geneator.GetNewGuid()).Return(_guid2).Repeat.Once();
            var target = new ResolveContext(_stubGuidGenerator, _logger);
            const string variableName = "ECD7A685-EDCC-474C-AD38-000000000001";
            var expected = _guid1;
            var actual = target.GetVariable(variableName);
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void ContextGetVariableUnderscoredTest()
        {            
            using(var testBooStrapper = new UnitTestsTemplate.TestBootStrapper(GetType()))
            {
                ////Prepare interface(s) that interface under test is dependent on by 
                ////letting the dependent (stub) interface(s) return dummy data to the interface under test
                //var stubSomeInterface = testBooStrapper.Container.Resolve<ISomeInterface>();
                //stubSomeInterface.Stub(i => i.GetSomeValue("somekey")).Return("somevalue1").Repeat.AtLeastOnce();
                const string attributeName = "_S_SomeVariable_S_";
                const string attributeNameUnderscored = "_S_SomeVariableU_S_";
                const string attributeVale = "Some value.2015-12-21";
                var expected = "Some_value_2015_12_21";
                var target = testBooStrapper.Container.Resolve<IResolveContext>();
                target.AddVariable(attributeName,attributeVale);
                var actual = target.GetVariable(attributeNameUnderscored);
                Assert.AreEqual(expected, actual);
            }
        }


        internal class TestBootStrapper: IDisposable
        {
            readonly ILog _logger;
            private IWindsorContainer _container;

            public TestBootStrapper(Type type)
            {
                _logger = new ConsoleOutLogger(type.Name,LogLevel.Info, true, false,false,"yyyy-MM-dd HH:mm:ss");
            }

            public IWindsorContainer Container
            {
                get
                {
                    if(_container == null)
                    {
                        _container = new WindsorContainer();
                        _container.Register(Component.For<IWindsorContainer>().Instance(_container));
            
                        //Configure logging
                        _container.Register(Component.For<ILog>().Instance(_logger));
            
                        //Manual override registrations for interfaces that the interface under test is dependent on
                        //_container.Register(Component.For<ISomeInterface>().Instance(MockRepository.GenerateStub<ISomeInterface>()));

                        //Factory registrations example:

                        //container.Register(Component.For<ITeamProviderFactory>().AsFactory());
                        //container.Register(
                        //    Component.For<ITeamProvider>()
                        //        .ImplementedBy<CsvTeamProvider>()
                        //        .Named("CsvTeamProvider")
                        //        .LifeStyle.Transient);
                        //container.Register(
                        //    Component.For<ITeamProvider>()
                        //        .ImplementedBy<SqlTeamProvider>()
                        //        .Named("SqlTeamProvider")
                        //        .LifeStyle.Transient);

                        ///////////////////////////////////////////////////////////////////
                        //Automatic registrations
                        ///////////////////////////////////////////////////////////////////
                        //
                        //   Register all command providers and attach logging interceptor
                        //
                        const string libraryRootNameSpace = "NCmdLiner.SolutionCreator.Library";
                        
                        //
                        //   Register all singletons found in the library
                        //
                        _container.Register(Classes.FromAssemblyContaining<CommandDefinition>()
                            .InNamespace(libraryRootNameSpace, true)
                            .If(type => Attribute.IsDefined(type, typeof(SingletonAttribute)))
                            .WithService.DefaultInterfaces().LifestyleSingleton());
                        
                        //
                        //   Register all transients found in the library
                        //
                        _container.Register(Classes.FromAssemblyContaining<CommandDefinition>()
                            .InNamespace(libraryRootNameSpace, true)
                            .WithService.DefaultInterfaces().LifestyleTransient());

                    }
                    return _container;
                }

            }

            ~TestBootStrapper()
            {
                Dispose(false);
            }

            public void Dispose()
            {
                Dispose(true);
            }

            protected virtual void Dispose(bool disposing)
            {
                if(disposing)
                {
                    if(_container != null)
                    {
                        _container.Dispose();
                        _container = null;
                    }
                }
            }
        }
    }
}
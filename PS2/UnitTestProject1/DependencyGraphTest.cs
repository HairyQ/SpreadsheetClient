using System;
using System.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;

namespace UnitTestProject1
{
    [TestClass]
    public class DependencyGraphTest
    {
        [TestMethod]
        public void testGetDependents()
        {
            ArrayList words = new ArrayList();
            words.Add("This");
            words.Add("Is");
            words.Add("Going");
            words.Add("TO");
            words.Add("b");
            words.Add("1");
            words.Add("HeL1");
            words.Add("OfaT3$t");

            DependencyGraph dG = new DependencyGraph();
            dG.AddDependency("Blorbo", "This");
            dG.AddDependency("Blorbo", "Is");
            dG.AddDependency("Blorbo", "Going");
            dG.AddDependency("Blorbo", "TO");
            dG.AddDependency("Blorbo", "b");
            dG.AddDependency("Blorbo", "1");
            dG.AddDependency("Blorbo", "HeL1");
            dG.AddDependency("Blorbo", "OfaT3$t");

            IEnumerable finalList 
        }

        [TestMethod]
        public void testGetDependees()
        {

        }

        [TestMethod]
        public void testSize()
        {
            DependencyGraph dG = new DependencyGraph();
            Assert.IsTrue(dG.Size == 0);
            dG.AddDependency("Blorbo", "word");
            Assert.IsTrue(dG.Size == 1);
            dG.AddDependency("Blorbo", "word");
            dG.AddDependency("Blorbo", "word");
            dG.AddDependency("Blorbo", "word");
            dG.AddDependency("Blorbo", "word");
            dG.AddDependency("Blorbo", "word");
            dG.AddDependency("Blorbo", "word");
            Assert.IsTrue(dG.Size == 1);
            dG.AddDependency("", "Tis");
            Assert.IsTrue(dG.Size == 2);
            dG.RemoveDependency("Blorbo", "word");
            Assert.IsTrue(dG.Size == 1);
            dG.AddDependency("Blorbo", "This");
            dG.AddDependency("Blorbo", "");
            dG.AddDependency("fdsaofpdsa89fdsafdsa", "dsajfka");
            dG.AddDependency("-1", "hey");
            dG.AddDependency("wow", "word");
            Assert.IsTrue(dG.Size == 6);
        }

        [TestMethod]
        public void testIndexer()
        {
            DependencyGraph dG = new DependencyGraph();
            Assert.IsTrue(dG[""] == 0);
            Assert.IsTrue(dG["Blorbo"] == 0);
            dG.AddDependency("Here", "Blorbo");
            Assert.IsTrue(dG["Blorbo"] == 1);
            dG.AddDependency("This", "Blorbo");
            Assert.IsTrue(dG["Blorbo"] == 2);
            dG.AddDependency("This", "Blorbo");
            dG.AddDependency("This", "Blorbo");
            Assert.IsTrue(dG["Blorbo"] == 2);
            dG.AddDependency("Hey", "Blorbo");
            dG.AddDependency("Wow", "Blorbo");
            dG.AddDependency("fdsa79fd6sa780f", "Blorbo");
            Assert.IsTrue(dG["Blorbo"] == 5);
            dG.RemoveDependency("fdsa79fd6sa780f", "Blorbo");
            Assert.IsTrue(dG["Blorbo"] == 4);
        }

        [TestMethod]
        public void TestAddDependency()
        {
            DependencyGraph dG = new DependencyGraph();
            Assert.IsFalse(dG.HasDependents("H"));
            dG.AddDependency("H", "Hello");
            Assert.IsTrue(dG.HasDependents("H"));

            dG.AddDependency("H", "Yo");
            Assert.IsTrue(dG.HasDependents("H"));
            Assert.IsTrue(dG.HasDependees("Yo"));
            Assert.IsTrue(dG.HasDependees("Hello"));

            dG = new DependencyGraph();
            Assert.IsFalse(dG.HasDependents("H"));
        }

        [TestMethod]
        public void TestRemoveDependency()
        {
            DependencyGraph dG = new DependencyGraph();
            Assert.IsFalse(dG.HasDependents("H"));
            dG.AddDependency("H", "Hello");
            Assert.IsTrue(dG.HasDependents("H"));
            dG.RemoveDependency("H", "Hello");
            Assert.IsFalse(dG.HasDependents("H"));

            dG = new DependencyGraph();
            dG.AddDependency("Here", "There");
            dG.AddDependency("Here", "Woah");
            dG.AddDependency("Here", "This");
            dG.AddDependency("There", "Foo");
            dG.AddDependency("There", "Blorbo");
            dG.AddDependency("Blorbo", "Hence");

            ArrayList words = new ArrayList();
            words.Add("There");
            words.Add("Woah");
            words.Add("This");

            IEnumerable dGWords = dG.GetDependents("Here");
            foreach(string s in dGWords)
            {
                Assert.IsTrue(words.Contains(s));
            }

            dG.RemoveDependency("Here", "There");
            
            Assert.IsFalse(dG.HasDependees("There"));
            Assert.IsTrue(dG.HasDependees("Woah"));
            Assert.IsTrue(dG.HasDependees("This"));
            Assert.IsTrue(dG.HasDependents("Here"));

            dG.RemoveDependency("Here", "Woah");
            dG.RemoveDependency("Here", "This");
            Assert.IsFalse(dG.HasDependents("Here"));
            Assert.IsTrue(dG.HasDependents("There"));
            Assert.IsTrue(dG.HasDependents("Blorbo"));

            dG.RemoveDependency("There", "Foo");
            dG.RemoveDependency("There", "Blorbo");
            Assert.IsFalse(dG.HasDependents("There"));
            Assert.IsTrue(dG.HasDependents("Blorbo"));

            dG.RemoveDependency("Blorbo", "Hence");
            Assert.IsFalse(dG.HasDependents("Here"));
            Assert.IsFalse(dG.HasDependents("There"));
            Assert.IsFalse(dG.HasDependents("Blorbo"));
        }

        [TestMethod]
        public void TestHasDependents()
        {
            DependencyGraph dG = new DependencyGraph();
            Assert.IsFalse(dG.HasDependents(""));

            dG.AddDependency("", "k");
            Assert.IsTrue(dG.HasDependents(""));

            dG = new DependencyGraph();
            Assert.IsFalse(dG.HasDependents(""));

            Assert.IsFalse(dG.HasDependents("Blorbo"));
            Assert.IsFalse(dG.HasDependents("Yes"));
            Assert.IsFalse(dG.HasDependents("3294872"));

            dG.AddDependency("Blorbo", "Yes");
            dG.AddDependency("Yes", "3294872");
            dG.AddDependency("3294872", "Fin");

            Assert.IsTrue(dG.HasDependents("Blorbo"));
            Assert.IsTrue(dG.HasDependents("Yes"));
            Assert.IsTrue(dG.HasDependents("3294872"));
            Assert.IsFalse(dG.HasDependents("Fin"));

        }

        [TestMethod]
        public void TestHasDependees()
        {
            DependencyGraph dG = new DependencyGraph();
            Assert.IsFalse(dG.HasDependees(""));

            dG.AddDependency("", "k");
            Assert.IsTrue(dG.HasDependees("k"));

            dG = new DependencyGraph();
            Assert.IsFalse(dG.HasDependees("k"));
            Assert.IsFalse(dG.HasDependees(""));

            Assert.IsFalse(dG.HasDependees("Blorbo"));
            Assert.IsFalse(dG.HasDependees("Yes"));
            Assert.IsFalse(dG.HasDependees("3294872"));

            dG.AddDependency("Blorbo", "Yes");
            dG.AddDependency("Yes", "3294872");
            dG.AddDependency("3294872", "Fin");

            Assert.IsFalse(dG.HasDependees("Blorbo"));
            Assert.IsTrue(dG.HasDependees("Yes"));
            Assert.IsTrue(dG.HasDependees("3294872"));
            Assert.IsTrue(dG.HasDependees("Fin"));
        }
    }
}

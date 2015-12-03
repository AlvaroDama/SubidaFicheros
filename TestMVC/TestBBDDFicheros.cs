using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SubidaFicheros.Models;

namespace TestMVC
{
    /// <summary>
    /// Summary description for TestBBDDFicheros
    /// </summary>
    [TestClass]
    public class TestBBDDFicheros : BasePruebas
    {
        public TestBBDDFicheros()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void PruebaConsultaFicherosAzure()
        {
            int esperado = 1;
            var recibido = db.Ficheros.Count(o => o.Tipo == "azure");  //lo suyo sería que lo hiciese usando un 
                                                                        //repositorio (pero en este proyecto no hay)
            
            Assert.AreEqual(esperado, recibido);
        }

        [TestMethod]
        public void PruebaConsultaFicherosBinarios()
        {
            var f = new Ficheros()
            {
                Datos = "",
                DatosB = new byte[] {},
                Nombre = "Borrar",
                TipoFichero = 1,
                Tipo = "azure"
            };

            int n = 0;

            db.Ficheros.Add(f);

            try
            {
                n = db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Assert.IsFalse(n==0);
        }
        
    }
}

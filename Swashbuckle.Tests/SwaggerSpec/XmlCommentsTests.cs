﻿using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Swashbuckle.Application;
using Swashbuckle.Dummy.Controllers;
using System;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Hosting;
using System.Web.Http.Routing;

namespace Swashbuckle.Tests.SwaggerSpec
{
    [TestFixture]
    public class XmlCommentsTests : HttpMessageHandlerTestsBase<SwaggerSpecHandler>
    {
        public XmlCommentsTests()
            : base("swagger/api-docs/{resourceName}")
        {}

        [SetUp]
        public void SetUp()
        {
            var swaggerSpecConfig = new SwaggerSpecConfig();
            swaggerSpecConfig.IncludeXmlComments(String.Format(@"{0}\XmlComments.xml", AppDomain.CurrentDomain.BaseDirectory));

            Handler = new SwaggerSpecHandler(swaggerSpecConfig);

            SetUpDefaultRouteFor<ProductsController>();
        }

        [Test]
        public void It_should_apply_action_summaries_to_operation_summaries()
        {
            var declaration = Get<JObject>("http://tempuri.org/swagger/api-docs/Products");

            var token = declaration.SelectToken("apis[0].operations[0].summary");
            Assert.IsNotNull(token);
            Assert.AreEqual("List all products", token.ToString());

            token = declaration.SelectToken("apis[0].operations[1].summary");
            Assert.IsNotNull(token);
            Assert.AreEqual(String.Empty, token.ToString());

            token = declaration.SelectToken("apis[0].operations[2].summary");
            Assert.IsNotNull(token);
            Assert.AreEqual("Create a new product", token.ToString());

            token = declaration.SelectToken("apis[1].operations[0].summary");
            Assert.IsNotNull(token);
            Assert.AreEqual("Retrieve product by unique Id", token.ToString());
        }

        [Test]
        public void It_should_apply_action_remarks_to_operation_notes()
        {
            var declaration = Get<JObject>("http://tempuri.org/swagger/api-docs/Products");

            var token = declaration.SelectToken("apis[0].operations[2].notes");
            Assert.IsNotNull(token);
            Assert.AreEqual("Requires admin priveleges", token.ToString());
        }

        [Test]
        public void It_should_apply_param_text_to_operation_parameter_descriptions()
        {
            var declaration = Get<JObject>("http://tempuri.org/swagger/api-docs/Products");

            var token = declaration.SelectToken("apis[0].operations[2].parameters[0].description");
            Assert.IsNotNull(token);
            Assert.AreEqual("New product details", token.ToString());
        }
        
        [Test]
        public void It_should_apply_responses_to_operation_response_messages()
        {
            var responseMessage = Get<JObject>("http://tempuri.org/swagger/api-docs/Products")
                .SelectToken("apis[0].operations[2].responseMessages[0]");

            Assert.IsNotNull(responseMessage);
            Assert.AreEqual("200", responseMessage["code"].ToString());
            Assert.AreEqual("It's all good!", responseMessage["message"].ToString());
        }
        
        [Test]
        public void It_should_apply_type_summaries_to_model_descriptions()
        {
            var declaration = Get<JObject>("http://tempuri.org/swagger/api-docs/Products");

            var token = declaration.SelectToken("models.Product.description");
            Assert.IsNotNull(token);
            Assert.AreEqual("Describes a product", token.ToString());
        }

        [Test]
        public void It_should_apply_property_summaries_to_model_property_descriptions()
        {
            var declaration = Get<JObject>("http://tempuri.org/swagger/api-docs/Products");

            var token = declaration.SelectToken("models.Product.properties.Id.description");
            Assert.IsNotNull(token);
            Assert.AreEqual("Uniquely identifies the product", token.ToString());
        }

        [Test]
        public void It_should_support_actions_marked_with_actionname()
        {
            SetUpCustomRouteFor<CustomActionNamesController>("{action}/{id}");
           
            var declaration = Get<JObject>("http://tempuri.org/swagger/api-docs/CustomActionNames");

            // Method name is DifferentMethodName and ActionName attribute is TestActionName
            // The XML file contains DifferentMethodName, so this verifies that the summary can still be found by that name.
            var token = declaration.SelectToken("apis[0].operations[0].summary");
            Assert.IsNotNull(token);
            Assert.AreEqual("Test ActionName attribute", token.ToString());

            // nickname takes from ApiDescription.ActionDescriptor.ActionName
            token = declaration.SelectToken("apis[0].operations[0].nickname");
            Assert.IsNotNull(token);
            Assert.AreEqual("CustomActionNames_TestActionName", token.ToString());
        }
    }
}
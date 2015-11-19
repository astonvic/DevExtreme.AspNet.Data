﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DevExtreme.AspNet.Data.Tests {

    public class DataSourceExpressionBuilderTests {
        public void Build_SkipTake() {
            var builder = new DataSourceExpressionBuilder<int> {
                Skip = 111,
                Take = 222
            };

            var expr = builder.Build(false);

            Assert.Equal("data.Skip(111).Take(222)", expr.Body.ToString());
        }

        [Fact]
        public void Build_Filter() {
            var builder = new DataSourceExpressionBuilder<int> {
                Filter = new object[] { "this", ">", 123 }
            };

            var expr = builder.Build(false);

            Assert.Equal("data.Where(obj => (obj > 123))", expr.Body.ToString());
        }

        [Fact]
        public void Build_CountQuery() {
            var builder = new DataSourceExpressionBuilder<int> {
                Skip = 111,
                Take = 222,
                Filter = new object[] { "this", 123 },
                Sort = new[] {
                    new SortingInfo { Selector = "this" }
                },
            };

            var expr = builder.Build(true);
            var text = expr.ToString();

            Assert.Contains("Where", text);
            Assert.DoesNotContain("Skip", text);
            Assert.DoesNotContain("Take", text);
            Assert.DoesNotContain("OrderBy", text);
            Assert.EndsWith(".Count()", text);
        }

        [Fact]
        public void Build_Sorting() {
            var builder = new DataSourceExpressionBuilder<Tuple<int, string>> {
                Sort = new[] {
                    new SortingInfo {
                        Selector="Item1"
                    },
                    new SortingInfo {
                        Selector = "Item2",
                        Desc=true
                    }
                }
            };

            var expr = builder.Build(false);
            Assert.Equal("data.OrderBy(obj => obj.Item1).ThenByDescending(obj => obj.Item2)", expr.Body.ToString());
        }
    }

}

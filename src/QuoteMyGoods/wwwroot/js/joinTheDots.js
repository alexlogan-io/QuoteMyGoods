/// <reference path="d3.d.ts"/>
/// <reference path="jquery.d.ts" />
var JoinTheDots;
(function (JoinTheDots) {
    var Graph = (function () {
        function Graph(targetArea) {
            this.helpers = new Helpers();
            this.width = 1000;
            this.height = 600;
            this.radius = 15;
            this.padding = 30;
            var self = this;
            this.dots = new Dots();
            this.svg = d3.select(targetArea).append("div").selectAll("svg")
                .data(d3.range(1).map(function () { return { x: self.width / 2, y: self.height / 2 }; }))
                .enter().append("svg")
                .attr("width", self.width)
                .attr("height", self.height)
                .on("mousedown", function () {
                self.mousedown(this, self);
            })
                .on("mousemove", function () {
                self.mousemove(this, self);
            })
                .on("contextmenu", function (d, i) {
                d3.event.preventDefault();
                // react on right-clicking
            });
            this.svg.selectAll("*").remove();
            this.lineFunction = d3.svg.line()
                .x(function (d) { return d[0]; })
                .y(function (d) { return d[1]; })
                .interpolate("linear");
            this.drag = d3.behavior.drag()
                .on("drag", function () {
                self.dragMove(this, self);
            });
            this.xScale = d3.scale.linear()
                .domain([-1000, 1000])
                .range([this.padding, this.width - this.padding * 2]);
            this.yScale = d3.scale.linear()
                .domain([-1000, 1000])
                .range([this.height - this.padding, this.padding]);
            this.xData = ["One", "Two", "Three", "Four", "Five"];
            this.xAxis = d3.svg.axis()
                .scale(this.xScale)
                .orient("bottom")
                .ticks(5)
                .tickFormat(function (d, i) {
                return self.xData[i];
            });
            this.yData = ["One", "Two", "", "Four", "Five"];
            this.yAxis = d3.svg.axis()
                .scale(this.yScale)
                .orient("left")
                .ticks(5)
                .tickFormat(function (d, i) {
                return self.yData[i];
            });
            this.createAxis(this.xAxis, this.yAxis, this.svg);
        }
        Graph.prototype.createAxis = function (xAxis, yAxis, svg) {
            //Create X axis
            svg.append("g")
                .attr("id", "x")
                .attr("class", "axis")
                .attr("transform", "translate(" + (this.padding / 2) + "," + (this.height - this.padding - ((this.height - (this.padding * 2)) / 2)) + ")")
                .call(xAxis);
            //Create Y axis
            svg.append("g")
                .attr("id", "y")
                .attr("class", "axis")
                .attr("transform", "translate(" + (this.padding + ((this.width - (this.padding * 2)) / 2)) + ",0)")
                .call(yAxis);
            svg.append("g")
                .attr("id", "pathContainer");
            svg.append("text")
                .attr("id", "coordText")
                .attr("transform", "translate(" + (this.width - (this.padding * 3) - 20) + "," + (this.height - this.padding + 20) + ")")
                .text("x:0" + " " + "y:0");
        };
        Graph.prototype.dragMove = function (context, self) {
            var point = d3.mouse(context);
            var x = point[0];
            var y = point[1];
            var arrayCount = d3.select(context).attr("pathIndex");
            if (x > self.padding + self.radius && x < ((self.width - (self.padding * 2)) + self.radius) && y < (self.height - self.padding) && y > self.padding) {
                if (d3.select(context).attr("class").indexOf("circle") >= 0) {
                    d3.select(context).attr("cx", x);
                    d3.select(context).attr("cy", y);
                    d3.select(context.parentNode.childNodes[1]).attr("transform", "translate(" + (x - 5) + "," + (y + 5) + ")");
                }
                else {
                    d3.select(context.parentNode.childNodes[0]).attr("cx", x);
                    d3.select(context.parentNode.childNodes[0]).attr("cy", y);
                    d3.select(context).attr("transform", "translate(" + (x - 5) + "," + (y + 5) + ")");
                }
                self.dots.UpdateDots(context, x, y, arrayCount);
            }
        };
        Graph.prototype.mousedown = function (context, self) {
            if (d3.event.button == 2) {
                if (d3.event.target.className.baseVal.indexOf("circle") >= 0 || d3.event.target.className.baseVal.indexOf("textT") >= 0) {
                    var index = Number($(context).attr("index"));
                    if (!isNaN(index)) {
                        $(self.dots.nodeArray[index][0]).parent().remove();
                        self.dots.nodeArray.splice(index, 1);
                        for (var i = index; i < self.dots.nodeArray.length; i++) {
                            var elm = $(self.dots.nodeArray[i][0]).parent()[0];
                            $(elm.firstChild).attr("index", i);
                            $(elm.lastChild).text(i + 1);
                        }
                        self.dots.nodeCount -= 1;
                    }
                }
            }
            else {
                if (d3.event.target.className.baseVal.indexOf("circle") === -1 && d3.event.target.className.baseVal.indexOf("textT") === -1 && d3.event.target.className.baseVal.indexOf("dotPath") === -1) {
                    var coordinates = [0, 0];
                    coordinates = d3.mouse(context);
                    var x = coordinates[0];
                    var y = coordinates[1];
                    var circle = self.drawCircle(self, x, y, self.dots.nodeCount, self.dots.pathIndex);
                    self.dots.nodeCount++;
                    self.dots.nodeArray.push(circle);
                }
            }
        };
        Graph.prototype.mousemove = function (context, self) {
            var coordinates = [0, 0];
            coordinates = d3.mouse(context);
            var x = Math.round(self.helpers.alexValToPixX(coordinates[0]));
            var y = Math.round(self.helpers.alexValToPixY(coordinates[1]));
            d3.select("#coordText").text("x:" + x + " " + "y:" + y);
        };
        Graph.prototype.insertRandom = function () {
            var self = this;
            for (var n = 0; n < 30; n++) {
                var x = this.helpers.alexPixToValX(Math.floor(Math.random() * 2000) - 1000);
                var y = this.helpers.alexPixToValY(Math.floor(Math.random() * 2000) - 1000);
                var circle = this.drawCircle(self, x, y, self.dots.nodeCount, self.dots.pathIndex);
                self.dots.nodeCount++;
                self.dots.nodeArray.push(circle);
            }
        };
        Graph.prototype.drawCircle = function (self, x, y, count, arrayCount) {
            var container = self.svg.append("g");
            var circle = container.append("circle")
                .attr("index", count)
                .attr("class", "circle")
                .attr("fill", "#B5E9EA")
                .attr("r", self.radius)
                .attr("cx", x)
                .attr("cy", y)
                .attr("pathIndex", arrayCount)
                .on("mousedown", function () {
                self.mousedown(this, self);
            })
                .call(self.drag);
            var text = container.append("text")
                .attr("class", "textT")
                .text((count + 1))
                .attr("transform", "translate(" + (x - 5) + "," + (y + 5) + ")")
                .attr("id", count)
                .attr("pathIndex", arrayCount)
                .attr("fill", "#00414A")
                .call(self.drag);
            return circle;
        };
        Graph.prototype.Save = function () {
            var stringLines = { dots: this.dots.allLines };
            console.log(JSON.stringify(stringLines));
            $.ajax({
                method: "POST",
                url: "/api/dots",
                data: JSON.stringify(stringLines),
                contentType: "application/json"
            }).fail(function (res) {
                console.log(res);
                console.log("failed");
            });
        };
        Graph.prototype.Load = function () {
            var self = this;
            $.ajax({
                method: "GET",
                url: "/api/dots"
            }).done(function (res) {
                self.dots.Reset();
                var dots = JSON.parse(res).dots;
                for (var j = 0; j < dots.length; j++) {
                    var nodeArray = dots[j];
                    for (var i = 0; i < nodeArray.length; i++) {
                        //dont render dots twice
                        var circle = self.drawCircle(self, nodeArray[i].x, nodeArray[i].y, i, j);
                        self.dots.nodeArray.push(circle);
                    }
                    self.dots.JoinTheDots();
                }
            });
        };
        return Graph;
    }());
    JoinTheDots.Graph = Graph;
    var Helpers = (function () {
        function Helpers() {
        }
        Helpers.prototype.alexValToPixY = function (inputValue) {
            return ((inputValue / 0.27) - (30 / 0.27) - 1000) * -1;
        };
        Helpers.prototype.alexPixToValY = function (inputValue) {
            return (((inputValue + 1000) * 0.27 + 30) - 600) * -1;
        };
        Helpers.prototype.alexValToPixX = function (inputValue) {
            return (inputValue / 0.455) - (45 / 0.455) - 1000;
        };
        Helpers.prototype.alexPixToValX = function (inputValue) {
            return (inputValue + 1000) * 0.455 + 45;
        };
        return Helpers;
    }());
    JoinTheDots.Helpers = Helpers;
    var Dots = (function () {
        function Dots() {
            this.lineData = [];
            this.nodeArray = [];
            this.pathIndex = 0;
            this.allLines = [];
            this.nodeCount = 0;
            this.helpers = new Helpers();
            this.lineFunction = d3.svg.line()
                .x(function (d) { return d.x; })
                .y(function (d) { return d.y; })
                .interpolate("linear");
        }
        ;
        Dots.prototype.JoinTheDots = function () {
            for (var i = 0; i < this.nodeArray.length; i++) {
                this.lineData.push({ "x": this.nodeArray[i].attr("cx"), "y": this.nodeArray[i].attr("cy") });
            }
            this.lineData.push({ "x": this.nodeArray[0].attr("cx"), "y": this.nodeArray[0].attr("cy") });
            var pathContainer = d3.select("#pathContainer");
            pathContainer.append("path")
                .attr("class", "dotPath")
                .attr("id", this.pathIndex)
                .attr("d", this.lineFunction(this.lineData))
                .attr("stroke-width", 2)
                .attr("fill", "#B5E9EA")
                .attr("opacity", "0.5")
                .attr("pathIndex", this.pathIndex);
            this.allLines[this.pathIndex] = this.lineData;
            this.lineData = [];
            this.nodeArray = [];
            d3.selectAll(".textT").remove();
            d3.selectAll(".circle").attr("r", 7)
                .classed("joined_circle", true).
                classed("circle", false);
            this.nodeCount = 0;
            $("#areaText").append('<a class="btn btn-default"></a>');
            $("#areaText a:last-child").addClass("areaTag").attr("pathIndex", this.pathIndex).click(function () {
                var self = this;
                d3.selectAll(".joined_circle").classed("selectedArea", false);
                $(".areaTag").removeClass("selectedArea");
                d3.selectAll(".dotPath").classed("selectedArea", false);
                $(".dotPath").each(function (index) {
                    if ($(this).attr("id") === $(self).attr("pathIndex")) {
                        d3.select(this).classed("selectedArea", true);
                        $(self).addClass("selectedArea");
                    }
                });
                $(".joined_circle").each(function () {
                    if (d3.select(this).attr("pathIndex") === $(self).attr("pathIndex")) {
                        d3.select(this).classed("selectedArea", true);
                    }
                });
            }).text("Area: " + (this.pathIndex + 1));
            this.pathIndex++;
        };
        Dots.prototype.Reset = function () {
            d3.selectAll("circle").remove();
            d3.selectAll(".textT").remove();
            d3.selectAll(".dotPath").remove();
            d3.select("#areaText").selectAll("*").remove();
            this.nodeCount = 0;
            this.nodeArray = [];
            this.lineData = [];
            this.pathIndex = 0;
        };
        Dots.prototype.UpdateDots = function (context, x, y, arrayCount) {
            var self = this;
            var lineData = this.allLines[arrayCount];
            if (lineData != undefined) {
                if (Number($(context).attr("index")) == 0) {
                    lineData[lineData.length - 1] = { "x": x, "y": y };
                }
                lineData[Number($(context).attr("index"))] = { "x": x, "y": y };
                var paths = d3.selectAll(".dotPath").each(function (d) {
                    var arCount = d3.select(this).attr("id");
                    if (arCount === arrayCount) {
                        d3.select(this).attr("d", self.lineFunction(lineData));
                        return false;
                    }
                });
            }
        };
        return Dots;
    }());
    JoinTheDots.Dots = Dots;
    $(document).ready(function () {
        var graph = new Graph("#d3QuestionArea");
        $("#JoinButton").click(function () {
            graph.dots.JoinTheDots();
        });
        $("#ResetButton").on("click", function () {
            graph.dots.Reset();
        });
        $("#SaveButton").on("click", function () {
            graph.Save();
        });
        $("#LoadButton").on("click", function () {
            graph.Load();
        });
        $("#RandomButton").on("click", function () {
            graph.insertRandom();
        });
    });
})(JoinTheDots || (JoinTheDots = {}));

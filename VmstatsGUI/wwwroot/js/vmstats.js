// The following sample code uses modern ECMAScript 6 features 
// that aren't supported in Internet Explorer 11.
// To convert the sample for environments that do not support ECMAScript 6, 
// such as Internet Explorer 11, use a transpiler such as 
// Babel at http://babeljs.io/. 
//
// See Es5-chat.js for a Babel transpiled version of the following code:



// TODO Implement a mechanism to add graphs to the client display whenever the server calls the clients methods. In this way the client can display lots of graphs instead of one
// TODO Whenever a new DSL is entered and submitted for processing to the server, assuming the return is success then clear the graph displays and wait for results
// TODO Ensure any charts displayed in an area are sorted by their title so they should always be in the same order given them same DSL





const connection = new signalR.HubConnectionBuilder()
    .withUrl("/vmstatsHub")
    .build();

// This method is called by the server
connection.on("ReceiveMessage", (user, message) => {
    const msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    const encodedMsg = user + " says " + msg;
    const li = document.createElement("li");
    li.textContent = encodedMsg;
    document.getElementById("messagesList").appendChild(li);
});


// This method draws a chart in a specified area of the screen
function drawChart(chartName, xData, yData) {
    var ctx = document.getElementById(chartName).getContext('2d');

    //line chart data
    var data = {
        labels: xData,
        datasets: [
            {
                label: "Usage",
                data: yData,
                backgroundColor: "blue",
                borderColor: "lightblue",
                fill: false,
                lineTension: 0,
                radius: 0
            }
        ]
    };

    //options
    var options = {
        responsive: true,
        maintainAspectRatio: false,
        title: {
            display: true,
            position: "top",
            text: "Title - TODO needs changing",
            fontSize: 18,
            fontColor: "#111"
        },
        legend: {
            display: true,
            position: "bottom",
            labels: {
                fontColor: "#333",
                fontSize: 16
            }
        }
    };

    console.dir(options);

    //create Chart class object
    var chart = new Chart(ctx, {
        type: "line",
        data: data,
        options: options
    });


}



// This method is called by the server to display a graph in the raw area
connection.on("DisplayGraph", (displayArea, xData, yData) => {
    console.log("In DisplayGraph");
    console.log("dsioplayArea = " + displayArea);
    console.dir(xData);
    console.dir(yData);
    drawChart(displayArea, xData, yData);
});

connection.start().catch(err => console.error(err.toString()));

// Add an event listener so when the button is pressed it invokes the server side code
document.getElementById("processButton").addEventListener("click", event => {
    console.log("In processButton - click");
    const fromDate = document.getElementById("FromDate").value;
    const toDate = document.getElementById("ToDate").value;
    const vmPattern = document.getElementById("VmPattern").value;
    const dsl = document.getElementById("Dsl").value;
    connection.invoke("Process", fromDate, toDate, vmPattern, dsl).catch(err => console.error(err.toString()));
    event.preventDefault();
});




// The following sample code uses modern ECMAScript 6 features 
// that aren't supported in Internet Explorer 11.
// To convert the sample for environments that do not support ECMAScript 6, 
// such as Internet Explorer 11, use a transpiler such as 
// Babel at http://babeljs.io/. 
//
// See Es5-chat.js for a Babel transpiled version of the following code:

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


// This method draws a chart
function drawChart(chartName, data) {
    var ctx = document.getElementById(chartName).getContext('2d');
/*
    new Chart(document.getElementById(chartName), {
        type: 'line',
        data: {
            labels: [1500, 1600, 1700, 1750, 1800, 1850, 1900, 1950, 1999, 2050],
            datasets: [{
                data: [86, 114, 106, 106, 107, 111, 133, 221, 783, 2478],
                label: "Africa",
                borderColor: "#3e95cd",
                fill: false
            }, {
                data: [282, 350, 411, 502, 635, 809, 947, 1402, 3700, 5267],
                label: "Asia",
                borderColor: "#8e5ea2",
                fill: false
            }
            ]
        },
        options: {
            title: {
                display: true,
                text: 'World population per region (in millions)'
            }
        }
    });
*/
    //line chart data
    var data = {
        labels: ["match1", "match2", "match3", "match4", "match5"],
        datasets: [
            {
                label: "TeamA Score",
                data: [10, 50, 25, 70, 40],
                backgroundColor: "blue",
                borderColor: "lightblue",
                fill: false,
                lineTension: 0,
                radius: 5
            },
            {
                label: "TeamB Score",
                data: [20, 35, 40, 60, 50],
                backgroundColor: "green",
                borderColor: "lightgreen",
                fill: false,
                lineTension: 0,
                radius: 5
            }
        ]
    };

    //options
    var options = {
        responsive: true,
        title: {
            display: true,
            position: "top",
            text: "Line Graph",
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



// This method is called by the server
connection.on("DisplayRaw", (message) => {
    console.log("In DisplayRaw" + message);

    drawChart("rawGraph", "");
});

connection.start().catch(err => console.error(err.toString()));

// This method invokes the server side code
document.getElementById("sendButton").addEventListener("click", event => {
    const user = document.getElementById("userInput").value;
    const message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", user, message).catch(err => console.error(err.toString()));
    event.preventDefault();
});




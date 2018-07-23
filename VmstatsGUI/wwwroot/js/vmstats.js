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
function drawChart(canvas, title, xData, yData) {
//    var ctx = document.getElementById(chartName).getContext('2d');
    var ctx = canvas.getContext('2d');

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
//            text: "Title - TODO needs changing",
            text: title,
            fontSize: 18,
            fontColor: "#111"
        },
        legend: {
            display: false,
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


function sortList(listName) {
    console.log("In SortList3");
    var mylist = document.getElementById('container-'+listName);
    var divs = mylist.getElementsByTagName('div');
    var listitems = [];
    for (i = 0; i < divs.length; i++) {
        listitems.push(divs.item(i));
    }
    listitems.sort(function (a, b) {
        if (b == null) return 1;
        var compA = a.getAttribute('id').toUpperCase();
        var compB = b.getAttribute('id').toUpperCase();
        return (compA < compB) ? -1 : (compA > compB) ? 1 : 0;
    });

    console.dir(listitems);

    for (i = 0; i < listitems.length; i++) {
        mylist.appendChild(listitems[i]);
    }
}


// This method is called by the server to display a graph in the raw area
connection.on("DisplayGraph", (isRaw, xData, yData, vmName, date, metricName) => {
    console.log("In DisplayGraph");
    console.log("isRaw = " + isRaw);
    console.log("vmName = " + vmName);
    console.log("date = " + date);
    console.log("metricName = " + metricName);
    console.dir(xData);
    console.dir(yData);

    var displayArea = (isRaw == true) ? "rawGraph" : "processedGraph";

    var title = vmName + "-" + date + "-" + metricName;

    // Determine which list to add the canvas to
    console.log("getting the list info");
    var listName = (isRaw == true) ? "rawList" : "processedList";
    var list = document.getElementById("container-"+listName);
    var entry = document.createElement('div');
//    var entry = document.createElement('li');
    entry.id = title;

    // Create a canvas element within the ordered list
    console.log("creating the canvas");
    var canvas = document.createElement('canvas');
    canvas.id = title;
//    canvas.width = auto;
    canvas.width = window.innerWidth;
    canvas.height = 400;
 //   canvas.width = 1224;
 //   canvas.height = 768;
 //   canvas.style.zIndex = 8;
 //   canvas.style.position = "absolute";
 //   canvas.style.border = "1px solid";
    console.log("canvas = " + canvas);

    // Add the new canvas to the list
    console.log("adding canvas to list");
    entry.appendChild(canvas);
    list.appendChild(entry);

    // Draw the chart and reorder the list
    drawChart(canvas, title, xData, yData);
    sortList(listName);
});

connection.start().catch(err => console.error(err.toString()));

// Add an event listener so when the button is pressed it invokes the server side code
document.getElementById("processButton").addEventListener("click", event => {
    console.log("In processButton - click");

    // Remove all children from the lists so the screen is clear for the new data to be added
    var list = document.getElementById("container-rawList");
    while (list.hasChildNodes()) {
        list.removeChild(list.firstChild);
    }
    var list = document.getElementById("container-processedList");
    while (list.hasChildNodes()) {
        list.removeChild(list.firstChild);
    }

    const fromDate = document.getElementById("FromDate").value;
    const toDate = document.getElementById("ToDate").value;
    const vmPattern = document.getElementById("VmPattern").value;
    const dsl = document.getElementById("Dsl").value;
    connection.invoke("Process", fromDate, toDate, vmPattern, dsl).catch(err => console.error(err.toString()));
    event.preventDefault();
});




"use strict"

var connection = new signalR.HubConnectionBuilder().withUrl("/ProgressDownload").build();

var readb = 0;

connection.on("Receive", (readbytes) => {
    //document.getElementById("progress-info").innerHTML = readbytes;
    readb = readbytes;
});

connection.start().then(() => {
});

setInterval(() => {
    connection.invoke("Send");
}, 2000);
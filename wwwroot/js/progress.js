"use strict"

var connection = new signalR.HubConnectionBuilder().withUrl("/ProgressDownload").build();

connection.on("Receive", (readbytes) => {
    document.getElementById("progress-info").innerHTML = readbytes;
});

connection.start().then(() => {
});

setInterval(() => {
    connection.invoke("Send");
}, 2000);
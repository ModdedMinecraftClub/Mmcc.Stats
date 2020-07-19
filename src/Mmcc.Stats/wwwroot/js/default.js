async function onSubmit() {
    let fromDate = document.getElementById("from").value;
    let toDate = document.getElementById("to").value;
    let mode = document.getElementById("mode-input").value;
    
    if (fromDate === '' || toDate === '') {
        alert("Date cannot be empty");
        return;
    }
    
    let apiResponse = await getData(fromDate, toDate);

    if (mode === 'Smoothed data') {
        createPlot(apiResponse, true);
    } else {
        createPlot(apiResponse, false);
    }
}

async function getData(fromDate, toDate) {
    //https://localhost:5001/api/playerbase-stats?from=2019-12-23&to=2020-01-03
    let response = await fetch(`https://localhost:5001/api/playerbase-stats?from=${fromDate}&to=${toDate}`);
    return await response.json();
}

function createPlot(data, isSmooth) {
    let traces = [];
    
    for (const serverData of data) {
        if (serverData.timesList.length !== 0) {
            if (isSmooth) {
                traces.push(createSmoothTrace(serverData));
            } else {
                traces.push(createRawTrace(serverData));
            }
        }
    }

    Plotly.newPlot('plot', traces);
}

function createRawTrace(serverData) {
    return {
        name: serverData.serverName,
        x: serverData.timesList,
        y: serverData.playersOnlineList,
        mode: 'lines',
        type: 'scatter'
    };
}

function createSmoothTrace(serverData) {
    return {
        name: serverData.serverName,
        x: serverData.timesList,
        y: smooth(serverData.playersOnlineList, 15),
        mode: 'lines',
        type: 'scatter'
    };
}
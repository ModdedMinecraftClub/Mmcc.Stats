async function onSubmit() {
    
    console.log(`from: ${document.getElementById("from").value}`)
    console.log(`to: ${document.getElementById("to").value}`)
    console.log(`mode: ${document.getElementById("mode-input").value}`)
    
    let f = await getRaw();
    
    console.log(f);
        
    let data = createTraces(f);
    
    console.log(data);

    Plotly.newPlot('plot', data);
}

async function getRaw() {
    let response = await fetch('https://localhost:5001/api/playerbase-stats?from=2019-12-23&to=2020-01-03');
    return await response.json();
}

function createTraces(data) {
    let traces = [];
    
    for (const serverData of data) {
        if (serverData.timesList.length !== 0) {
            let trace = {
                name: serverData.serverName,
                x: serverData.timesList,
                y: smooth(serverData.playersOnlineList, 15),
                mode: 'lines',
                type: 'scatter'
            }
            
            traces.push(trace);
        }
    }
    
    return traces;
}
// initialise UI components
document.addEventListener('DOMContentLoaded', function() {
    const elems = document.querySelectorAll('select');
    const instances = M.FormSelect.init(elems);
});

async function onSubmit() {
    
    console.log(`from: ${document.getElementById("from").value}`)
    console.log(`to: ${document.getElementById("to").value}`)
    console.log(`mode: ${document.getElementById("mode-input").value}`)
    
    let trace1 = {
        x: [1, 2, 3, 4],
        y: [10, 15, 13, 17],
        mode: 'markers',
        type: 'scatter'
    };

    let trace2 = {
        x: [2, 3, 4, 5],
        y: [16, 5, 11, 9],
        mode: 'lines',
        type: 'scatter'
    };

    let trace3 = {
        x: [1, 2, 3, 4],
        y: [12, 9, 15, 12],
        mode: 'lines+markers',
        type: 'scatter'
    };

    let data = [trace1, trace2, trace3];

    Plotly.newPlot('plot', data);
}

async function getRaw() {
    
}

async function createTraces(data) {
    
}
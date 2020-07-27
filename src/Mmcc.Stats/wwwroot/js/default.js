document.addEventListener('DOMContentLoaded', () => {
    // Get all "navbar-burger" elements
    const navbarBurgers = Array.prototype.slice.call(document.querySelectorAll('.navbar-burger'), 0);

    // Check if there are any navbar burgers
    if (navbarBurgers.length > 0) {

        // Add a click event on each of them
        navbarBurgers.forEach( el => {
            el.addEventListener('click', () => {

                // Get the target from the "data-target" attribute
                const target = el.dataset.target;
                const targetDom = document.getElementById(target);

                // Toggle the "is-active" class on both the "navbar-burger" and the "navbar-menu"
                el.classList.toggle('is-active');
                targetDom.classList.toggle('is-active');

            });
        });
    }
});

async function onSubmit() {
    let fromDate = document.getElementById("from").value;
    let toDate = document.getElementById("to").value;
    let mode = document.getElementById("mode-input").value;
    
    if (fromDate === '' || toDate === '') {
        alert("Date cannot be empty");
        return;
    }
    
    if (fromDate > toDate) {
        alert("From date cannot be bigger than To date");
        return;
    }

    let response = await fetch(`/api/playerbase-stats?from=${fromDate}&to=${toDate}`);
    
    if (!response.ok) {
        alert("API HTTP-Error" + response.status);
        return;
    }
    
    let responseData = await response.json();

    if (mode === 'Smoothed data') {
        createPlot(responseData, true);
    } else {
        createPlot(responseData, false);
    }
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

    let layout = {
        autosize: true // set autosize to rescale
    };    
    let config = {responsive: true}

    Plotly.newPlot('plot', traces, layout, config);
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

const vehicleList = document.querySelector("#vehicleList");

const loadVehicles = async() => {
    const url = 'http://localhost:5000/api/v1/vehicles/'

    const response = await fetch(url);

    if(response.status === 200){
        const data = await response.json();

        let html = '';

        data.map(vehicle => {
            html += `
                <div class="gallery-card">
                    <div>${vehicle.manufacturer} ${vehicle.model}</div>
                    <img src="${vehicle.imageUrl}" alt="${vehicle.model}"/>
                    <div><span>${vehicle.model}</span>&nbsp;<span>${vehicle.modelYear}</span></div>
                    <a class="btn btn-dark">Se mer</a>
                </div>
            `
        });

        vehicleList.innerHTML = html;
    }
}

loadVehicles();
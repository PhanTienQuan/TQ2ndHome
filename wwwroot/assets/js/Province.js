fetch('https://vn-public-apis.fpo.vn/provinces/getAll?limit=-1')
    .then(response => response.json())
    .then(data => {
        let provinces = data.data.data
        provinces.map(value => document.getElementById('provinces').innerHTML += `<option value="${value.code}">${value.name}</option>`)
    })

    .catch(error => {
        console.error('Lỗi khi gọi api: ', error)
    });
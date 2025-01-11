window.addEventListener('DOMContentLoaded', (event) => {
    getVisitCount();
})

const functionApiUrl = 'https://getresumecontador.azurewebsites.net/api/GetResumeCounter?code=ZdBNsBpejzoh9ZGdaezjqtMoASbHDQ3o_5Zsje-YvAS2AzFu3WPImg%3D%3D';
const localFunctionApi = 'http://localhost:7071/api/GetResumeCounter'
const getVisitCount = () => {
    let count = 30;
    fetch(functionApiUrl).then(response => {
        return response.json()
    }).then(response =>{
        console.log("Website called function API.");
        count = response.count;
        document.getElementById("counter").innerText = count;
    }).catch(function(error){
        console.log(error);
    });
    return count;
}
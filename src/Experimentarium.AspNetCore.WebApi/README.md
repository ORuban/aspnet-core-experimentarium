## Build and run the app with Docker

Assuming that you are in the root of the repository, do 

```console
cd src/Experimentarium.AspNetCore.WebApi
docker build -t experimentarium_aspnetcore_webapi .
docker run -it --rm -p 5000:80 --name experimentarium_webapi experimentarium_aspnetcore_webapi 
```

After the container with the application starts, go to `http://localhost:5000` in your web browser.
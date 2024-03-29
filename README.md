# Movie API
The project was created for the subject WAP (Web applications). The goal of the project was to create a web application, which can visualize movies, actors, and directors as a network. 
Users will be then able to search for their favorite movie and trace other movies they might like based on the directors/actors they liked. 

The backend for this application was written using .NET and the frontend was created using Vue.js. This project was meant for teams of two people. I was responsible for the backend, but I helped out wherever I could with the frontend.
The backend consists of API. Users in the application can sign up and store their data. Therefore API has to provide authentification and authorization services. For data storage, API uses MSSQL to which the application is connected using EntityFrameworkCore.
We applied a code-first approach to this project (Data/database.md). API also serves as a bridge for Vue.js application to access IMDb and Tmdb APIs which require authentification to use it. 
This is done this way because we did not want to store our access tokens in the front-end application.

As mentioned above, the frontend application is written in Vue.js and was created using Visual Studio's template. For visualization of the movie networks we used the package "v-network-graph".

![image](https://github.com/xsojka04/MovieApiApp/assets/52315948/264a50a9-b0ba-4639-a61b-f6ed3469c14c)

![image](https://github.com/xsojka04/MovieApiApp/assets/52315948/6b2f540b-c26e-42d7-b920-071891b02c7f)

![image](https://github.com/xsojka04/MovieApiApp/assets/52315948/3a6d2cad-f29b-4081-9b44-41e2b9a80b9e)

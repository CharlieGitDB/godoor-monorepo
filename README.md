# Godoor
This application is used to manage, and organize properties you own or are involved with.  This could be used for a timeshare, or a shared cabin, or an landlord property organization. This project is being created as an example of a portfolio of development work. 

**The main goals of this project will be:**
1. Have visuals describing and explaining the architecture
2. Building multiple frontends with different libraries with SSR and SPA (Next, Nuxt, Angular, possibly SvelteKit)
3. Building out a stable backend using many cloud (Azure) services such as KeyVault, API Management, B2C, App Services, Functions, Messaging Services (Service Bus, Event Grid).
4. Handling the entire infrastructure through IaC, with DevOps pipelines and/or Github Actions

**Ideas & Goals that may or may not be done eventually:**
1. Creating backends using technologies I feel like trying. (Nest.js, Flask, Spring)?
2. Creating an entirely serverless backend architecture
3. Adding a visually pleasing scrolling intro page using Three.js similar to how Apple displays their products
4. Using a VM to host the servers and databases to have more control over the networking and scaling
5. Create "native" mobile apps using Android & Kotlin, Flutter, and React Native
6. Add all reuseable components to Storybook

**File Structure:**

```
.
└── Godoor/
    ├── web/
    │   ├── next/
    │   │   ├── app
    │   │   ├── dashboard
    │   │   ├── user-management
    │   │   └── shared
    │   ├── nuxt/
    │   │   ├── app
    │   │   ├── dashboard
    │   │   ├── user-management
    │   │   └── shared
    │   └── angular/
    │       ├── app
    │       ├── dashboard
    │       ├── user-management
    │       └── shared
    ├── services/
    │   ├── cs/
    │   │   ├── identity
    │   │   ├── message
    │   │   └── calendar
    │   ├── js
    │   └── java
    └── mobile/
        ├── android
        ├── flutter
        └── react-native
  ```
The file structure is organized in three parts.  
  
**Web:**  
*Web -> [Library]*  
The directories immediately under Web are the different front end web techologies.
  - Next for SSR with React
  - Nuxt for SSR with Vue
  - Angular for a standard SPA  

*Web -> [Library] -> [Micro frontend]*  
The directories under the specific front end libraries are for micro frontends which would be their own isolated instances/projects using the library.  Depending on how large the app becomes, it can be useful to allow different parts of the front end to be isolated instances that can be deployed on their own.  Example:
  - an app instance could handle the main guts of the app
  - a dashboard instance that handles only the dashboard ui
  - a user-management instance which only handles the user settings and ui such as profile, preferences etc  

**Services:**  
*Services -> [Programming Language/Framework]*  
The directories immediately under the services directory are the different programming languages that will be use for server side logic.
  - CS, This will be mostly .NET web APIs using common technologies such as EF core
  - JS, this could be any node frameworks using typescript (eg Nest.js, Koa, Express)
  - Java, this would be mostly if not entirely Spring web APIs
  - Python, this could be web APIs using Flask  
 
 *Services -> [Programming Language/Framework] -> [Microservice/misc]*  
 The directories under the server side frameworks will predominitly consist of web APIs using the common web frameworks for the parent folders programming language.  This directory may also hold pipelines and azure function code. Example:
  - Identity, which would be a microservice that handles the user data
  - Message, which may be a websocket communication for instant messaging
  - Calendar, a microservice that may only handle calendar data  


 **Mobile:**  
 *Mobile -> [Framework]*  
 The directories immediately under the mobile directory are frameworks that compile to native mobile apps.
  - Android, this would be an android app using the Kotlin language to create a native mobile android app
  - Flutter, this would be a cross platform mobile app
  - React Native, this would also be a cross plantform mobile app

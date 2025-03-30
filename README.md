# BayersHealthcare API

## 🚀 Deployed URL  
🔗 https://bayershealthcareportal.azurewebsites.net

**user name: 8553655890**

**password:  qwerty**

## 🛠 Technology Stack  
- **.NET Core**  
- **MediatR** (CQRS Pattern)  
- **NoSQL** (CosmosDB / MongoDB)  
- **Dependency Injection**  
- **REST API**  

## 📦 CI/CD with GitHub Actions  
This project is configured with **GitHub Actions** for continuous integration and deployment (CI/CD) to **Azure App Service**.  
The workflow includes:  
1. **Building** the application.  
2. **Deploying** to **Azure Web App** using `azure/webapps-deploy@v2`.  

### 🏠 GitHub Actions Workflow  
- The CI/CD pipeline is defined in `.github/workflows/BayersHealthcare.yml`.  
- On push to `main`, the app is automatically **built and deployed** to **Azure App Service**.  
- GitHub Secrets store deployment credentials (`AZURE_WEBAPP_PUBLISH_PROFILE`).  

## 📝 API Features  
👉 **CQRS Pattern** using **MediatR**  
👉 **NoSQL Database Integration**  
👉 **RESTful API Design**  
👉 **Dependency Injection for Loose Coupling**  

---

### 🚀 How to Run Locally  
1. Clone the repository:  
   ```sh
   git clone https://github.com/yourusername/BayersHealthcare.git
   ```
2. Navigate to the project directory:  
   ```sh
   cd BayersHealthcare
   ```
3. Restore dependencies:  
   ```sh
   dotnet restore
   ```
4. Run the application:  
   ```sh
   dotnet run
   ```



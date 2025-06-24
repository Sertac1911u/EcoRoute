# ♻️ EcoRoute - Smart Waste Collection and Route Optimization System

> Developed as part of the 2024-2025 senior year graduation project at Tekirdağ Namık Kemal University, Department of Computer Engineering, and supported by the **TÜBİTAK 2209-A** Research Support Program.

## 📌 Project Summary

**EcoRoute** is an IoT and AI-powered system developed to make the urban waste collection process more efficient and environmentally friendly. The project includes:
- Measuring waste bin fill levels with Raspberry Pi-based sensor hardware.
- Real-time monitoring and management via mobile (Flutter) and web (Blazor) applications.
- AI-assisted waste fill level prediction and route optimization to save time and fuel.
- Visualization through Google Maps API.

## 🧠 Literature Review

This project was shaped based on the insights of the following academic works:
- Sahoo et al. (2004), **route optimization in waste collection**.
- Gürcan & Açıksöz (2023), **smart city applications and data visualization**.
- Medvedev et al. (2015), **HTTP-based communication in IoT devices**.
- Sosunova & Porras (2022), **systematic review of smart waste systems**.
- Cormen et al. (2009), **fundamentals of algorithms and dynamic programming**.

## 🛠️ Technologies Used

| Area | Technology |
|------|------------|
| Web Application | ASP.NET Blazor |
| Mobile Application | Flutter (iOS & Android) |
| Hardware | Raspberry Pi Zero WH, LDR, Laser Module |
| Data Communication | HTTP protocol |
| Map & Visualization | Google Maps API |
| Database | SQL Server |
| Authentication | JWT Token |
| Architecture | Microservices + Docker |
| Artificial Intelligence | LSTM |

## 🔍 System Architecture

> ![microservices (1)](https://github.com/user-attachments/assets/8efa827e-3e75-4f6c-bfe5-ef21467e699a)


- Raspberry Pi devices transmit fill level data to the central server via HTTP.
- The server is managed through a Blazor-based web UI.
- Mobile users (field workers) are provided with optimized routes.
- Routes are optimized based on traffic, bin fill status, or minimum distance.

## 🧠 AI Algorithms Used

### 🔹 LSTM (Long Short-Term Memory)
- Predicts bin fill levels using historical data.
- Supports proactive route planning before bin overflows.

## 📱 Application Screenshots

### Web Interface (Blazor)
> ![routepage0](https://github.com/user-attachments/assets/38a8e76f-b641-460f-aa48-da14df2ce2f6)
> ![routepage1](https://github.com/user-attachments/assets/0232ede3-6eee-431d-957d-425bb3573888)
> ![userpagenot](https://github.com/user-attachments/assets/fff0c3cc-85c9-4b86-8166-af902060c2ce)




### Mobile App (Flutter)
> ![1](https://github.com/user-attachments/assets/70af344e-cd06-4afa-b83c-450624d9e29a)
> ![2](https://github.com/user-attachments/assets/55318ccf-4dc0-4911-9fc1-d09e6fb57859)



## 🧪 Hardware & Physical Setup

- Each Raspberry Pi device is connected to 4 LDR sensors and a laser module.
- Fill level logic:
  - All sensors active → 0% full
  - Bottom 1 sensor blocked → 25% full
  - Bottom 2 sensors blocked → 50% full
  - etc.
- GPIO pins are used for real-time sensor readings.

## ✅ Project Contributions

- Environmental sustainability
- Efficient waste collection routes
- Visualized data for monitoring and decision-making
- Seamless mobile and web integration
- Academic publication potential

## 📚 References

- Gürcan, C., & Açıksöz, S. (2023). *Smart Waste Management and Case Studies*. Kent Akademisi.
- Sahoo, S. P. et al. (2004). *Routing Optimization for Waste Management*.
- Medvedev, A. et al. (2015). *Waste Management as an IoT-enabled Service in Smart Cities*.
- Cormen, T. H. et al. (2009). *Introduction to Algorithms*.
- Sosunova, I., & Porras, J. (2022). *IoT-enabled Smart Waste Management Systems*.
- Dreyfus, S. E. *Dynamic Programming Principles and Applications*.

## 🧑‍💻 Developer

> **Sertaç YILDIRIM**  
Tekirdağ Namık Kemal University - Computer Engineering  
📬 sertac1911u@gmail.com  
🔗 [LinkedIn](https://www.linkedin.com/in/sertaç-yıldırım/)

## 🧪 TÜBİTAK 2209-A Research Project

> This study is supported by **TÜBİTAK 2209-A**, a national research funding program for university students in Turkey in the year 2024.

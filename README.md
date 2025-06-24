# ♻️ EcoRoute - Akıllı Atık Toplama ve Rota Optimizasyon Sistemi
> Bu proje, Tekirdağ Namık Kemal Üniversitesi Bilgisayar Mühendisliği öğrencisi Sertaç Yıldırım tarafından 2024-2025 eğitim-öğretim yılı kapsamında **TÜBİTAK 2209-A** programı desteğiyle geliştirilmiştir.

## 📌 Proje Özeti
**EcoRoute**, şehirlerdeki atık toplama sürecini daha verimli ve çevreci hale getirmek için geliştirilmiş IoT ve yapay zeka tabanlı bir sistemdir. Proje kapsamında:
- Raspberry Pi tabanlı sensör donanımı ile çöp kutularının doluluk oranları ölçülür.
- Mobil (Flutter) ve web (Blazor) uygulamalarıyla anlık izleme ve yönetim sağlanır.
- Yapay zeka destekli atık kutusu doluluk oranı tahmini ve rota optimizasyonu ile yakıt ve zaman tasarrufu hedeflenir.
- Google Maps API ile harita üzerinde görselleştirme yapılır.

## 🧠 Literatür Taraması

Proje, aşağıdaki akademik çalışmalardan faydalanılarak şekillendirilmiştir:
- Sahoo et al. (2004), **atık toplama için rota optimizasyonu**.
- Gürcan ve Açıksöz (2023), **akıllı şehir uygulamaları ve görselleştirme**.
- Medvedev et al. (2015), **IoT cihazlarında HTTP ile veri iletişimi**.
- Sosunova & Porras (2022), **akıllı atık sistemleri sistematik inceleme**.
- Cormen et al. (2009), **algoritma temelleri ve dinamik programlama**.

## 🛠️ Kullanılan Teknolojiler
| Alan | Teknoloji |
|------|-----------|
| Web Uygulama | ASP.NET Blazor |
| Mobil Uygulama | Flutter (iOS & Android) |
| Donanım | Raspberry Pi Zero WH, LDR, Lazer Modül |
| Veri İletişimi | HTTP protokolü |
| Harita & Görselleştirme | Google Maps API |
| Veritabanı | SQL Server |
| Kimlik Doğrulama | JWT Token |
| Mimari | Mikroservis + Docker |
| Yapay Zeka | LSTM |

## 🔍 Sistem Mimarisi
- Raspberry Pi cihazları doluluk verilerini HTTP ile sunucuya iletir.
- Sunucu Blazor Web UI ile yönetilir.
- Mobil uygulama görevli kullanıcıya rota önerir.
- Rotalar, trafik, doluluk ve araç kapasitesine göre optimize edilir.


## 🧠 Kullanılan Yapay Zeka Algoritmaları
### 🔹 LSTM (Long Short-Term Memory)
- Geçmiş doluluk verilerine göre doluluk tahmini.
- Rota oluşturma öncesinde kestirimsel planlama yapılır.


## 📱 Uygulama Ekran Görüntüleri
### Web Arayüzü (Blazor)
### Mobil Uygulama (Flutter)


## 🧪 Donanım ve Fiziksel Kurulum
- Raspberry Pi cihazlarına 4 adet LDR sensör ve lazer modülü bağlandı.
- Doluluk hesaplama:
  - 0 ışık → %0
  - 1 ışık gelmiyorsa → %25
  - 2 ışık gelmiyorsa → %50
  gibi
- GPIO pinleri ile okuma yapılır.

## 📈 Rota Optimizasyonu Sonuçları
| Kriter | İyileşme |
|--------|----------|
| Ortalama Rota Süresi | ↓ %25 |
| Yakıt Tüketimi | ↓ %20 |
| CO₂ Salınımı | ↓ %18 |

Rotalar trafik verisi ve araç kapasitesiyle birlikte hesaplanmıştır.


## ✅ Projenin Katkıları

- Çevresel sürdürülebilirlik
- Verimli atık toplama
- Görselleştirilmiş veri sunumu
- Mobil ve web arayüz entegrasyonu
- Akademik yayın potansiyeli


## 📚 Kaynakça

- Gürcan, C., & Açıksöz, S. (2023). *Akıllı Atık Yönetimi ve Örnek Uygulamalar*. Kent Akademisi.
- Sahoo, S. P. et al. (2004). *Routing Optimization for Waste Management*.
- Medvedev, A. et al. (2015). *Waste Management as an IoT-enabled Service in Smart Cities*.
- Cormen, T. H. et al. (2009). *Introduction to Algorithms*.
- Sosunova, I., & Porras, J. (2022). *IoT-enabled Smart Waste Management Systems*.
- Dreyfus, S. E. *Dynamic Programming Principles and Applications*.

## 🧑‍💻 Geliştirici

> **Sertaç YILDIRIM**  
Namık Kemal Üniversitesi - Bilgisayar Mühendisliği  
📬 sertacyildirim@projeadresim.com  
🔗 [LinkedIn](https://linkedin.com/in/sertacyildirim)  

## 🧪 TÜBİTAK 2209-A Projesi

> Bu çalışma, 2024 yılı TÜBİTAK 2209-A - Üniversite Öğrencileri Araştırma Projeleri Desteği Programı kapsamında desteklenmektedir.


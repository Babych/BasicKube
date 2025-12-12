# Basickube Deployment on AKS with TLS (nip.io)

This project demonstrates a simple Kubernetes deployment on AKS, exposed via an Ingress using **nip.io**, with automatic TLS certificates issued by **cert-manager** and Let's Encrypt (staging or production).

---

## Features

- Kubernetes Deployment & Service
- Ingress with nip.io domain
- TLS via cert-manager
- Optional self-signed certificates for local development
- GitHub Actions compatible for CI/CD deployment

---

## Prerequisites

- Azure Kubernetes Service (AKS) cluster
- `kubectl` configured for the cluster
- NGINX Ingress Controller installed
- cert-manager installed in the cluster

```bash
kubectl apply -f https://github.com/cert-manager/cert-manager/releases/latest/download/cert-manager.yaml
GitHub Actions runner (optional for CI/CD)

Deployment Steps
1. ClusterIssuer (Let's Encrypt staging)
Create a ClusterIssuer for staging certificates:

yaml
Copy code: https://github.com/Babych/BasicKube/blob/main/deploy/ingress%20configs/cluster-issuer.yaml

bash
kubectl apply -f cluster-issuer.yaml

2. Deployment & Service
Create your Deployment and Service for basickube:

Copy code:
https://github.com/Babych/BasicKube/blob/main/deploy/deployment.yaml
https://github.com/Babych/BasicKube/blob/main/deploy/service.yaml
https://github.com/Babych/BasicKube/blob/main/deploy/ingress.yaml

Apply them in CI/CD: https://github.com/Babych/BasicKube/blob/main/.github/workflows/cicd.yml

Check certificate status:

bash
Copy code
kubectl describe certificate basickube-tls -n basickube-ns
kubectl get secret basickube-tls -n basickube-ns

3. Access the Application
HTTP: http://basickube.172.199.209.31.nip.io/weatherforecast

HTTPS (staging): https://basickube.172.199.209.31.nip.io/weatherforecast

Chrome may show "Not Secure" if using url with IP adress. Edge may display it as secure.

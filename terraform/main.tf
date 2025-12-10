provider "azurerm" {
features {}
}


resource "azurerm_resource_group" "rg" {
name = "demo-rg"
location = "West Europe"
}


resource "azurerm_kubernetes_cluster" "aks" {
name = "demo-aks"
location = azurerm_resource_group.rg.location
resource_group_name = azurerm_resource_group.rg.name
dns_prefix = "demok8s"


default_node_pool {
name = "default"
node_count = 1
vm_size = "Standard_B1s" # минимальна тестова нода
}


identity {
type = "SystemAssigned"
}
}


output "kube_config" {
value = azurerm_kubernetes_cluster.aks.kube_config_raw
sensitive = true
}
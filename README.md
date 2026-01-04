# ⚡ Miner Control Center (MCC)

> **"A high-frequency telemetry engine engineered to bridge the gap between Enterprise Dashboards and Physical ASIC Hardware."**

### 📥 **Download Options:**
* **Direct GitHub Download:** [MCC_App.zip (v1.0)](https://github.com/AntonioDevTech/MinerControlCenter/raw/refs/heads/main/MCC_App.zip.zip)
* **Official Web Portfolio:** [titanalfapro.org](https://titanalfapro.org) *(View full project details and alternate download)*

---

## 📖 Executive Summary
The **Miner Control Center (MCC)** is a bespoke C# .NET console application architected to solve a critical problem in large-scale crypto mining operations known as **Vendor Fragmentation**.

The issue is that Whatsminers return JSON data while Antminers return unstructured text strings. Standard web interfaces are slow, single-threaded, and often crash under the load of managing mixed fleets.

**The Solution:**
I engineered a multi-threaded telemetry engine that bypasses the HTTP layer entirely. By opening raw **TCP Sockets (Port 4028)** directly to the hardware, the MCC injects JSON-RPC payloads to extract granular metrics. It pulls chip temperatures, fan RPMs, voltage, and error codes directly from the firmware and normalizes them into a unified real-time command-line dashboard.

---

## 🏗️ System Architecture & Logic Flow
The diagram below illustrates the flow of data from the physical silicon (ASIC Chips) through the network layer, parsed by the MCC Engine, and rendered on the Dashboard.

```mermaid
graph TD
    %% --- COLOR DEFINITIONS (Senior Architect Style) ---
    classDef hardware fill:#fff9c4,stroke:#fbc02d,stroke-width:2px,color:#e65100;
    classDef transport fill:#e3f2fd,stroke:#1565c0,stroke-width:2px,color:#0d47a1;
    classDef engine fill:#e8f5e9,stroke:#2e7d32,stroke-width:2px,color:#1b5e20;
    classDef ui fill:#f3e5f5,stroke:#7b1fa2,stroke-width:2px,color:#4a148c;

    %% --- LAYER 1: PHYSICAL HARDWARE ---
    subgraph "Layer 1: Physical Hardware"
        direction TB
        Sensors["🌡️ Thermal & Fan Sensors"]:::hardware
        RawData["Raw Voltage/Temp Data"]:::hardware
        
        subgraph "Mixed Fleet"
            Whatsminer["⚒️ Whatsminer M30S++"]:::hardware
            Antminer["⚒️ Antminer S19 Pro"]:::hardware
        end
    end

    %% --- LAYER 2: NETWORK TRANSPORT ---
    subgraph "Layer 2: Network Transport"
        TCPSocket["⚡ Raw TCP Socket (Port 4028)"]:::transport
        Payload["📦 JSON-RPC Payload"]:::transport
    end

    %% --- LAYER 3: MCC ENGINE ---
    subgraph "MCC Telemetry Engine (C#)"
        direction TB
        Regex["🧠 Universal Regex Parser"]:::engine
        Polling["🔄 Async Polling Loop (500ms)"]:::engine
        Safety["🛡️ Thermal Safety Logic (>85°C)"]:::engine
    end

    %% --- LAYER 4: USER INTERFACE ---
    subgraph "Layer 4: User Interface"
        Dashboard["🖥️ CLI Live Dashboard"]:::ui
        Inventory["🗃️ SQL Inventory"]:::ui
    end

    %% --- LOGIC FLOWS ---
    %% Data Acquisition
    Polling --"Injects {'command': 'stats'}"--> TCPSocket
    TCPSocket --"Bypasses HTTP"--> Whatsminer
    TCPSocket --"Bypasses HTTP"--> Antminer
    
    %% Hardware Response
    Whatsminer --"Returns JSON Stream"--> Payload
    Antminer --"Returns String Stream"--> Payload
    Payload --"Stream"--> Regex

    %% Data Processing
    Sensors --> RawData
    RawData --> Whatsminer
    RawData --> Antminer

    %% Logic & Safety
    Regex --"Normalize Data"--> Polling
    Polling --"Check Thresholds"--> Safety
    Safety --"Emergency Shutdown"--> TCPSocket
    Polling --"Render Data"--> Dashboard
    Dashboard --"Log History"--> Inventory

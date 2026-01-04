# ⚡ Titan MCC: Miner Control Center & Telemetry Engine

> **"A high-frequency telemetry engine engineered to bridge the gap between Enterprise Cloud dashboards and Physical ASIC Hardware."**

### 🔗 Status: [Active Development]

---

## 📖 Executive Summary
The **Miner Control Center (MCC)** is a bespoke C# .NET console application architected to solve a critical problem in large-scale crypto mining operations: **Vendor fragmentation.**

Whatsminers return JSON data. Antminers return unstructured text strings. Standard web interfaces are slow, single-threaded, and crash under load. 

**The Solution:** I engineered a multi-threaded telemetry engine that bypasses the HTTP layer entirely. By opening raw **TCP Sockets (Port 4028)** directly to the hardware, the MCC injects JSON-RPC payloads to extract granular metrics—Chip Temperatures, Fan RPMs, Voltage, and Error Codes—normalizing them into a unified, real-time command-line dashboard.

---

## 🏗️ System Architecture & Logic Flow
The diagram below illustrates the flow of data from the physical silicon (ASIC Chips) through the network layer, parsed by the Titan Engine, and rendered on the Dashboard.

```mermaid
graph TD
    %% --- STYLE DEFINITIONS ---
    classDef hardware fill:#fff9c4,stroke:#fbc02d,stroke-width:2px,color:#e65100;
    classDef network fill:#e3f2fd,stroke:#1565c0,stroke-width:2px,color:#0d47a1;
    classDef logic fill:#e8f5e9,stroke:#2e7d32,stroke-width:2px,color:#1b5e20;
    classDef ui fill:#f3e5f5,stroke:#7b1fa2,stroke-width:2px,color:#4a148c;

    %% --- PHYSICAL LAYER ---
    subgraph "Layer 1: Physical Hardware"
        MinerA["⚒️ Whatsminer M30S++"]:::hardware
        MinerB["⚒️ Antminer S19 Pro"]:::hardware
        Sensors["🌡️ Thermal & Fan Sensors"]:::hardware
    end

    %% --- NETWORK LAYER ---
    subgraph "Layer 3: Network Transport"
        TCP["🔌 Raw TCP Socket (Port 4028)"]:::network
        JSON["📦 JSON-RPC Payload"]:::network
    end

    %% --- LOGIC LAYER ---
    subgraph "MCC Telemetry Engine (C#)"
        Poller["🔄 Async Polling Loop (500ms)"]:::logic
        Parser["🧠 Universal Regex Parser"]:::logic
        Watchdog["🛡️ Thermal Safety Logic (>85°C)"]:::logic
    end

    %% --- PRESENTATION LAYER ---
    subgraph "Layer 7: User Interface"
        CLI["🖥️ CLI Live Dashboard"]:::ui
        DB[("🗄️ SQL Inventory")]:::ui
    end

    %% --- DATA FLOW ---
    Sensors --"Raw Voltage/Temp Data"--> MinerA
    Poller --"Injects: {'command': 'stats'}"--> TCP
    TCP --"Bypasses HTTP"--> MinerA
    MinerA --"Returns Telemetry"--> JSON
    JSON --"Stream"--> Parser
    Parser --"Normalize Data"--> CLI
    Parser --"Check Thresholds"--> Watchdog
    Watchdog --"Emergency Shutdown"--> TCP

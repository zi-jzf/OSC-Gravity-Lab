# Interactive Sand Sculpture VFX via OSC

<video src="https://github.com/user-attachments/assets/db6fdc1a-8653-4483-866f-edeccfcc9d25" autoplay loop muted playsinline width="40%"></video>

<video src="https://github.com/user-attachments/assets/56382042-80e0-49fc-9e0d-4a9c24cf2f1e" autoplay loop muted playsinline width="40%"></video>

スマートフォンのセンサーを利用し、リアルタイムに3D彫刻を崩壊・修復させるインタラクティブアート。
OSC通信を用いたフィジカルなデバイス連携と、VFX Graphによるパーティクル制御を組み合わせています。

## 概要

ZIG SIMアプリから送信されるデバイスの傾き情報（OSC）を受信し、画面内の胸像彫刻を砂のように崩壊させます。
地面と水平から垂直にデバイスを戻すと、崩れた砂が逆再生されるように元の彫刻の姿へと修復されます。

## 技術スタック

- Unity
- VFX Graph
- OSC (OSCJack)
- C#
- ZIGSIM (iOS App)

## セットアップと使用方法

### ZIGSIMの設定

* アプリを起動し、SETTINGSタブから以下を設定します。
* `Device UUID` : **`iphone`** (※Unity側の設定と一致させる)
* `IP Address` : 実行するPCのローカルIPアドレス
* `Port Number` : **`9000`**
* SENSORタブで、`Gravity`を有効にし、STARTを押す。

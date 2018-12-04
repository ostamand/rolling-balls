```
python freeze_graph.py --input_graph saved_models/v1/graph.pb --input_checkpoint saved_models/v1/agent.ckpt --output_node_names action --output_graph saved_models/v1/agent_v1.bytes
```
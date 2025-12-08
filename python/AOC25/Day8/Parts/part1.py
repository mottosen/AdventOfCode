from pathlib import Path

from Parts.util.handlers import build_graph, handle_edge


class Part1:
    def __init__(self, input_file: Path):
        with open(input_file, "r") as f:
            self.input_data = f.readlines()

        self.junction_limit = 1000

    def get_day_solution(self):
        vertices, edges = build_graph(self.input_data)

        # we build the circuits
        circuits = {}
        counter = 1
        for v1, v2 in edges[: self.junction_limit]:
            counter = handle_edge(circuits, counter, v1, v2)

        circuits = [len(circuit) for circuit in circuits.values()]
        circuits.sort(reverse=True)

        # we count the length of requested circuits
        total_length = 1
        for circuit in circuits[:3]:
            total_length *= circuit

        return str(total_length)

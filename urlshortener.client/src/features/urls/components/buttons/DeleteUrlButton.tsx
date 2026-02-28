import {useState} from "react";
import {Button} from "../../../../components/ui/buttons/Button.tsx";

interface Props {
    urlId: number;
    onDelete: (id: number) => Promise<boolean>;
}

export const DeleteUrlButton = ({ urlId, onDelete }: Props) => {
    const [isLoading, setIsLoading] = useState(false);

    const handleDelete = async () => {
        if (!confirm("Are you sure you want to delete this URL?")) return;
        setIsLoading(true);
        await onDelete(urlId);
        setIsLoading(false);
    };

    return (
        <Button
            variant="danger"
            size="sm"
            onClick={handleDelete}
            isLoading={isLoading}
        >
            Delete
        </Button>
    );
};